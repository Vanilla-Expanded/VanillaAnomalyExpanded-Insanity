using Verse;

namespace VAEInsanity
{
    public class Hediff_Sacrificial : HediffWithComps
    {
        public override bool ShouldRemove => base.ShouldRemove 
            || pawn.MentalStateDef != DefsOf.VAEI_Sacrificial;

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (pawn.MentalStateDef == DefsOf.VAEI_Sacrificial)
            {
                pawn.MentalState.RecoverFromState();
            }
        }
    }
}
