using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class ThoughtWorker_SanityEffectWithInhumanized : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.Inhumanized() || p.TryGetSanity(out var need) is false)
            {
                return ThoughtState.Inactive;
            }

            if (need.CurLevel <= 0.25f)
            {
                return ThoughtState.Inactive;
            }

            if (need.CurLevel <= 0.50f)
            {
                return ThoughtState.ActiveAtStage(0);
            }

            if (need.CurLevel <= 0.75f)
            {
                return ThoughtState.ActiveAtStage(1);
            }

            if (need.CurLevel <= 1.0f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            return ThoughtState.Inactive;
        }
    }
}
