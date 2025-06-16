using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class JobDriver_ViciousMock : JobDriver_MadnessInteraction
    {
        public override InteractionDef Interaction => DefsOf.VAEI_ViciousMockery;
    }

    public class JobDriver_Slander : JobDriver_MadnessInteraction
    {
        public override InteractionDef Interaction => DefsOf.VAEI_Slander;
    }

    public abstract class JobDriver_MadnessInteraction : JobDriver
    {
        private const TargetIndex TargetInd = TargetIndex.A;

        private Pawn Target => (Pawn)(Thing)pawn.CurJob.GetTarget(TargetIndex.A);

        public abstract InteractionDef Interaction {  get; }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            yield return InsultingSpreeDelayToil();
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            toil.socialMode = RandomSocialMode.Off;
            yield return toil;
            yield return InteractToil();
        }

        private Toil InteractToil()
        {
            return Toils_General.Do(delegate
            {
                if (pawn.interactions.TryInteractWith(Target, Interaction))
                {
                    if (pawn.MentalState is MentalState_Madness state)
                    {
                        state.lastInsultTicks = Find.TickManager.TicksGame;
                    }
                    else if (pawn.MentalState is MentalState_InsultingSpree spree)
                    {
                        spree.lastInsultTicks = Find.TickManager.TicksGame;
                        if (spree.target == Target)
                        {
                            spree.insultedTargetAtLeastOnce = true;
                        }
                    }
                }
            });
        }

        private Toil InsultingSpreeDelayToil()
        {
            Action action = delegate
            {
                var lastInsultTicks = pawn.MentalState is MentalState_Madness mentalState_InsultingSpree 
                ? mentalState_InsultingSpree.lastInsultTicks
                : (pawn.MentalState as MentalState_InsultingSpree).lastInsultTicks;
                if (Find.TickManager.TicksGame - lastInsultTicks >= 1200)
                {
                    pawn.jobs.curDriver.ReadyForNextToil();
                }
            };
            Toil toil = ToilMaker.MakeToil("InsultingSpreeDelayToil");
            toil.initAction = action;
            toil.tickAction = action;
            toil.socialMode = RandomSocialMode.Off;
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            return toil;
        }
    }
}
