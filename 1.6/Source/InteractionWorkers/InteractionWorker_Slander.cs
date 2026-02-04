using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_Slander : InteractionWorker_SanityInsult
    {
        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);

            foreach (Pawn otherPawn in recipient.Map.mapPawns.AllPawnsSpawned)
            {
                if (otherPawn != recipient && otherPawn != initiator && otherPawn.relations.OpinionOf(recipient) != 0)
                {
                    otherPawn.needs?.mood?.thoughts?.memories.TryGainMemory(DefsOf.VAEI_Suspicious, recipient);
                }
            }
        }
    }
}
