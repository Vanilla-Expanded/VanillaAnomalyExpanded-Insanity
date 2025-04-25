using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VAEInsanity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }
    [HotSwappable]
    public class Need_Sanity : Need
    {
        public bool shouldBeVisible;
        public override bool ShowOnNeedList => shouldBeVisible;
        public List<SanityChangeRecord> records = new List<SanityChangeRecord>();
        public HashSet<Pawn> killedShamblers = new HashSet<Pawn>();
        public TraitDef rehumanizedTrait;
        public Need_Sanity(Pawn pawn) : base(pawn)
        {
        }

        public override string GetTipString()
        {
            var baseTip = base.GetTipString();
            var passiveEffectsTip = new StringBuilder();
            var recentEventsTip = new StringBuilder();

            if (records.Any())
            {
                var explanation = new StringBuilder(DefsOf.VAEI_SanityGainPerDay.Worker.GetExplanationUnfinalized(StatRequest.For(pawn),
                    DefsOf.VAEI_SanityGainPerDay.toStringNumberSense));

                if (StatPart_SanityNeedOffset.TryGetSanityValue(pawn, out _, out var explanationPart2))
                {
                    explanation.AppendTagged(explanationPart2.ToTaggedString());
                }

                var regex = new Regex(@"-?\d+(\.\d+)?%");
                string ColorizePercentage(Match match)
                {
                    var value = float.Parse(match.Value.TrimEnd('%'));
                    return value < 0 ? match.Value.Colorize(NeedsCardUtility.MoodColorNegative) 
                        : match.Value.Colorize(NeedsCardUtility.MoodColor);
                }

                if (explanation.Length > 0)
                {
                    passiveEffectsTip.AppendLine("VAEI_PassiveEffects".Translate());
                    passiveEffectsTip.AppendTagged(regex.Replace(explanation.ToString(), new MatchEvaluator(ColorizePercentage)));
                }

                var recentEvents = records.Skip(Math.Max(0, records.Count() - 10)).ToList();
                if (recentEvents.Any())
                {
                    recentEventsTip.AppendLine("VAEI_MostRecentEvents".Translate());
                }
                var groupedRecords = new List<(SanityChangeRecord record, int count)>();
                for (int i = 0; i < recentEvents.Count; i++)
                {
                    var currentRecord = recentEvents[i].Copy();
                    int count = 1;
                    while (i + 1 < recentEvents.Count && recentEvents[i + 1].reason.Equals(currentRecord.reason))
                    {
                        currentRecord.effect += recentEvents[i + 1].effect;
                        currentRecord.ticksOccured = recentEvents[i + 1].ticksOccured;
                        count++;
                        i++;
                    }
                    groupedRecords.Add((currentRecord, count));
                }
                foreach (var (record, count) in groupedRecords.AsEnumerable().Reverse())
                {
                    string timeAgo = (Find.TickManager.TicksGame - record.ticksOccured).ToStringTicksToPeriod();
                    var effectEntry = record.effect.ToStringPercentSigned("F2").Colorize(record.effect < 0 ?
                        NeedsCardUtility.MoodColorNegative : NeedsCardUtility.MoodColor);
                    var entry = "VAEI_SanityRecord".Translate(timeAgo, record.reason, effectEntry);
                    if (count > 1)
                    {
                        entry += $" (x{count})";
                    }
                    recentEventsTip.AppendLineTagged("- " + entry);
                }
            }
            List<string> tips = new List<string>();

            if (!string.IsNullOrWhiteSpace(baseTip))
                tips.Add(baseTip.TrimEndNewlines());
            if (passiveEffectsTip.Length > 0)
                tips.Add(passiveEffectsTip.ToString().TrimEndNewlines());
            if (recentEventsTip.Length > 0)
                tips.Add(recentEventsTip.ToString().TrimEndNewlines());

            var combinedTip = string.Join("\n\n", tips);
            return combinedTip;
        }

        public override void SetInitialLevel()
        {
            bool isHoraxCult = false;
            if (Current.Game?.World != null && Find.FactionManager != null)
            {
                isHoraxCult = pawn.Faction == Find.FactionManager.OfHoraxCult;
            }

            if (isHoraxCult
                || pawn.HasTrait(TraitDefOf.VoidFascination)
                || pawn.HasTrait(TraitDefOf.Occultist)
                || pawn.Inhumanized())
            {
                CurLevel = Rand.Range(0.0f, 0.15f);
            }
            else
            {
                CurLevel = 1;
            }
        }

        public void GainSanity(float value, string reason = null, bool doMessage = true)
        {
            value = PostProcess(value);
            var oldValue = CurLevel;
            CurLevel += value;
            if (reason != null)
            {
                if (shouldBeVisible && PawnUtility.ShouldSendNotificationAbout(pawn) && doMessage)
                {
                    if (value > 0 && value >= 0.05f)
                    {
                        Messages.Message("VAEI_SanityGainMessage".Translate(reason.UncapitalizeFirst(), CurLevel.ToStringPercent(), pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                    else if (value < 0 && value <= -0.02f)
                    {
                        Messages.Message("VAEI_SanityLossMessage".Translate(reason.UncapitalizeFirst(), CurLevel.ToStringPercent(), pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }

                records.Add(new SanityChangeRecord
                {
                    ticksOccured = Find.TickManager.TicksGame,
                    reason = reason,
                    effect = value
                });
            }

            if (pawn.Inhumanized() is false && oldValue > 0 && CurLevel <= 0)
            {
                if (ModsConfig.IdeologyActive && pawn.Ideo.HasPrecept(PreceptDefOf.Inhumanizing_Required))
                {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.HumanityBreak, "VAEI_SanityLoss".Translate());
                }
                else
                {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(DefsOf.VAEI_Madness, "VAEI_SanityLoss".Translate());
                }
            }
        }

        public void AddEffect(ref float val, float effect, StringBuilder explanation, string message)
        {
            effect = PostProcess(effect);
            val += effect;
            explanation.AppendLine(message + ": " + effect.ToStringPercentSigned("F2"));
        }

        private float PostProcess(float value)
        {
            if (value < 0)
            {
                value *= pawn.GetStatValue(DefsOf.VAEI_SanityLossMultiplier);
            }
            else if (value > 0)
            {
                value *= pawn.GetStatValue(DefsOf.VAEI_SanityGainMultiplier);
            }
            value *= pawn.GetStatValue(DefsOf.VAEI_SanityMultiplier);
            if (CurLevel + value > 1)
            {
                value = 1 - CurLevel;
            }
            else if (CurLevel + value < 0)
            {
                value = -CurLevel;
            }
            return value;
        }

        public override void NeedInterval()
        {
            var valuePerDay = pawn.GetStatValue(DefsOf.VAEI_SanityGainPerDay) / 60000f;
            var valuePerInterval = valuePerDay * 150f; // Calculate value per 150 ticks
            if (valuePerInterval != 0)
            {
                GainSanity(valuePerInterval);
            }
        }

        public override float CurLevel 
        {   
            get => base.CurLevel;
            set
            {
                var oldValue = base.CurLevel;
                base.CurLevel = value;
                if (value < 0.95f && shouldBeVisible is false)
                {
                    Notify_LosingSanity();
                }
                else if (value >= 1)
                {
                    shouldBeVisible = false;
                    if (oldValue < 1)
                    {
                        var inhumanized = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Inhumanized);
                        if (inhumanized != null)
                        {
                            pawn.needs?.mood?.thoughts?.memories.TryGainMemory(DefsOf.VAEI_Rehumanized);
                            pawn.health.RemoveHediff(inhumanized);
                            var rehumanizedTraits = DefDatabase<SanityEffectsDef>.AllDefs.SelectMany(x => x.rehumanizationTraits ?? new List<TraitDef>()).Distinct().ToList();
                            rehumanizedTrait = rehumanizedTraits.Where(x => pawn.HasTrait(x) is false).RandomElement();
                            var trait = new Trait(rehumanizedTrait);
                            pawn.story.traits.GainTrait(trait);
                            var traitName = trait.CurrentData.GetLabelFor(pawn);
                            var traitDesc = trait.CurrentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn).Resolve();
                            Find.LetterStack.ReceiveLetter("VAEI_Rehumanized".Translate(pawn.Named("PAWN")), 
                                "VAEI_RehumanizedDesc".Translate(traitName, traitDesc, pawn.Named("PAWN")), LetterDefOf.PositiveEvent, pawn);
                        }
                    }
                }
            }
        }

        public void Notify_LosingSanity()
        {
            shouldBeVisible = true;
            if (PawnUtility.ShouldSendNotificationAbout(pawn) && PawnGenerator.IsBeingGenerated(pawn) is false)
            {
                Find.LetterStack.ReceiveLetter("VAEI_Sanity".Translate(), "VAEI_SanityLetterDesc".Translate(pawn.Named("PAWN")), LetterDefOf.NegativeEvent, pawn);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref shouldBeVisible, "shouldBeVisible");
            Scribe_Collections.Look(ref records, "records", LookMode.Deep);
            Scribe_Collections.Look(ref killedShamblers, "killedShamblers", LookMode.Reference);
            Scribe_Defs.Look(ref rehumanizedTrait, "rehumanizedTrait");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                records ??= new List<SanityChangeRecord>();
                killedShamblers ??= new HashSet<Pawn>();
            }
        }
    }
}
