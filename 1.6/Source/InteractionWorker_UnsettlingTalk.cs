using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_UnsettlingTalk : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            return 0f;
        }
    }
}
