using Verse;

namespace VAEInsanity
{

    public class HediffComp_CycleFocusDistracted : HediffComp
    {
        private int nextTransitionTick;

        public override void CompPostMake()
        {
            base.CompPostMake();
            SetNextTransition();
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Find.TickManager.TicksGame >= nextTransitionTick)
            {
                ToggleHediff();
                SetNextTransition();
            }
        }

        private void SetNextTransition()
        {
            nextTransitionTick = Find.TickManager.TicksGame + (parent.def == DefsOf.VAEI_Focused ? Rand.RangeInclusive(4000, 36000) : Rand.RangeInclusive(24000, 40000));
        }

        private void ToggleHediff()
        {
            Pawn pawn = parent.pawn;
            HediffDef newHediffDef = (parent.def == DefsOf.VAEI_Focused) ? DefsOf.VAEI_Distracted : DefsOf.VAEI_Focused;
            Hediff newHediff = HediffMaker.MakeHediff(newHediffDef, pawn);
            pawn.health.AddHediff(newHediff);
            pawn.health.RemoveHediff(parent);
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref nextTransitionTick, "HediffComp_CycleFocusDistracted_nextTransitionTick");
        }
    }
}
