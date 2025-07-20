using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    [HotSwappable]
    public class MentalState_Sacrificial : MentalState
    {
        public int nextSelfHarmTicks;
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            var sacrificial = HediffMaker.MakeHediff(DefsOf.VAEI_SacrificialHediff, pawn);
            pawn.health.AddHediff(sacrificial);
            nextSelfHarmTicks = new IntRange(300, 500).RandomInRange;
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            nextSelfHarmTicks -= delta;
            if (nextSelfHarmTicks <= 0)
            {
                nextSelfHarmTicks = new IntRange(300, 500).RandomInRange;
                InteractionWorker_VoidSelfHarm.DoSelfHarm(pawn);
            }
        }

        public override void PostEnd()
        {
            base.PostEnd();
            var sacrificial = pawn.health.hediffSet.GetFirstHediffOfDef(DefsOf.VAEI_SacrificialHediff);
            if (sacrificial != null)
            {
                pawn.health.RemoveHediff(sacrificial);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextSelfHarmTicks, "nextSelfHarmTicks");
        }
    }
}
