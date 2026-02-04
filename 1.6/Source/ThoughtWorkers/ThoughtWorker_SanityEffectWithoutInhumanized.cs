using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class ThoughtWorker_SanityEffectWithoutInhumanized : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.Inhumanized() || p.TryGetSanity(out var need) is false)
            {
                return ThoughtState.Inactive;
            }
            if (need.CurLevel > 0.75f)
            {
                return ThoughtState.Inactive;
            }
            if (need.CurLevel >= 0.50f)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            if (need.CurLevel >= 0.25f)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            if (need.CurLevel > 0.0f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            if (need.CurLevel == 0.0f && p.MentalStateDef == DefsOf.VAEI_Madness)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            return ThoughtState.Inactive;
        }
    }
}
