using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class ThoughtWorker_Restrained : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.Wears(DefsOf.VAEI_Apparel_StraitJacket))
            {
                return ThoughtState.ActiveAtStage(0);
            }
            return ThoughtState.Inactive;
        }
    }
}
