using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_SanityInsult : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.TryGetSanity(out var sanity))
            {
                if (sanity.CurLevel <= 0.5f)
                {
                    return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
                }
            }
            return 0f;
        }
    }
}
