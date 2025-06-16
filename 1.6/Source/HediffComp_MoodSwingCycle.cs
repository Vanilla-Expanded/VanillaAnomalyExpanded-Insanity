using Verse;

namespace VAEInsanity
{
    public class HediffComp_MoodSwingCycle : HediffComp
    {
        private int nextTransitionTick;
        public int CurrentStage { get; private set; }

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
                CycleStage();
                SetNextTransition();
            }
        }

        private void SetNextTransition()
        {
            nextTransitionTick = Find.TickManager.TicksGame + Rand.RangeInclusive(60000, 900000);
        }

        private void CycleStage()
        {
            int newStage;
            do
            {
                newStage = Rand.RangeInclusive(0, 3);
            } while (newStage == CurrentStage); // Ensure the new stage is different from the current stage

            CurrentStage = newStage;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref nextTransitionTick, "HediffComp_MoodSwingCycle_nextTransitionTick");
        }
    }
}
