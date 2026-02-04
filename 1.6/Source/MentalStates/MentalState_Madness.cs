using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    [HotSwappable]
    public class MentalState_Madness : MentalState_WanderOwnRoom
    {
        private int ticksToRamble, ticksToMock, ticksToSland, ticksToSelfHarm;
        private static List<Pawn> candidates = new List<Pawn>();
        public Pawn pawnTarget;
        public int lastInsultTicks = -999999;

        private static readonly IntRange RecoveryDurationTicksRange = new IntRange(GenDate.TicksPerDay, GenDate.TicksPerDay * 3);
        private static readonly IntRange RambleTicksRange = new IntRange(1200, 6000);

        public override void PostStart(string reason)
        {
            forceRecoverAfterTicks = RecoveryDurationTicksRange.RandomInRange;
            ticksToRamble = RambleTicksRange.RandomInRange;
            ticksToMock = SetTicks(4f, 8f);
            ticksToSland = SetTicks(4f, 8f);
            ticksToSelfHarm = SetTicks(8f, 12);
            base.PostStart(reason);
        }

        private static int SetTicks(float min, float max)
        {
            return (int)new FloatRange(min, max).RandomInRange * GenDate.TicksPerHour;
        }

        public override void MentalStateTick(int delta)
        {
            if (!pawn.Awake() || pawn.Suspended)
            {
                return;
            }
            if (pawn.IsHashIntervalTick(30, delta))
            {
                age += 30;
                if (age >= forceRecoverAfterTicks)
                {
                    RecoverFromState();
                    return;
                }
            }
            if (pawn.Spawned)
            {
                if (ticksToRamble <= 0)
                {
                    ticksToRamble = RambleTicksRange.RandomInRange;
                    SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn, InteractionDefOf.InhumanRambling);
                }
                else if (pawn.jobs.curDriver is not JobDriver_MadnessInteraction)
                {
                    if (pawn.Downed is false)
                    {
                        ticksToRamble -= delta;
                        ticksToMock -= delta;
                        ticksToSland -= delta;
                        ticksToSelfHarm -= delta;
                        if (ticksToMock <= 0)
                        {
                            ticksToMock = SetTicks(4f, 8f);
                            if (TryFindNewTarget())
                            {
                                StartJob(DefsOf.VAEI_ViciousMockJob);
                            }
                        }
                        else if (ticksToSland <= 0)
                        {
                            ticksToSland = SetTicks(4f, 8f);
                            if (TryFindNewTarget())
                            {
                                StartJob(DefsOf.VAEI_SlanderJob);
                            }
                        }
                        else if (ticksToSelfHarm <= 0)
                        {
                            ticksToSelfHarm = SetTicks(8f, 12);
                            if (VAEInsanityModSettings.selfHarmEnabled)
                            {
                                InteractionWorker_VoidSelfHarm.DoSelfHarm(pawn);
                            }
                        }
                    }
                }
            }
        }

        private void StartJob(JobDef job)
        {
            pawn.jobs.StopAll();
            pawn.jobs.StartJob(JobMaker.MakeJob(job, pawnTarget));
            Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
        }

        public override void PostEnd()
        {
            base.PostEnd();
            var actions = new List<(Action action, float chance)>();
            actions.Add((delegate
            {
                pawn.health.AddHediff(HediffDefOf.Inhumanized);
                Find.LetterStack.ReceiveLetter("VAEI_Inhumanized".Translate(pawn.Named("PAWN")), 
                    "VAEI_InhumanizedDesc".Translate(pawn.Named("PAWN")), LetterDefOf.NegativeEvent, pawn);
            
            }, 0.66f));
            actions.Add((delegate
            {
                pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.Catharsis);
                if (VAEInsanityModSettings.recoveringFromMadness.TryGetEffect(out var effect))
                {
                    if (pawn.TryGetSanity(out var sanity))
                    {
                        sanity.GainSanity(effect, "VEAI_RecoveringFromMadness".Translate());
                    }
                }
                Find.LetterStack.ReceiveLetter("VAEI_Recovered".Translate(pawn.Named("PAWN")),
                    "VAEI_RecoveredDesc".Translate(pawn.Named("PAWN")), LetterDefOf.PositiveEvent, pawn);
            }, 0.24f));
            if (VAEInsanityModSettings.selfHarmEnabled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                actions.Add((delegate
                {
                    pawn.health.AddHediff(HediffDefOf.Inhumanized);
                    Find.LetterStack.ReceiveLetter("VAEI_Sacrificial".Translate(pawn.Named("PAWN")),
                        "VAEI_SacrificialDesc".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn);
                    pawn.mindState.mentalStateHandler.TryStartMentalState(DefsOf.VAEI_Sacrificial);
                }, 0.10f));
            }

            if (actions.TryRandomElementByWeight(x => x.chance, out var action))
            {
                action.action();
            }
        }

        private bool TryFindNewTarget()
        {
            InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, candidates);
            bool result = candidates.TryRandomElement(out pawnTarget);
            candidates.Clear();
            return result;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToRamble, "ticksToRamble", 0);
            Scribe_Values.Look(ref ticksToMock, "ticksToMock", 0);
            Scribe_Values.Look(ref ticksToSland, "ticksToSland", 0);
            Scribe_Values.Look(ref ticksToSelfHarm, "ticksToSelfHarm", 0);
            Scribe_References.Look(ref pawnTarget, "pawnTarget");
            Scribe_Values.Look(ref lastInsultTicks, "lastInsultTicks", 0); 
        }
    }
}
