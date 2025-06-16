using RimWorld;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_VoidSelfHarm : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            return 0f;
        }

        public static void DoSelfHarm(Pawn pawn)
        {
            if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn, DefsOf.VAEI_VoidSelfHarm);
                var outsideParts = pawn.health.hediffSet.GetNotMissingParts()
                    .Where(x => x.depth == BodyPartDepth.Outside).ToList();
                var parts = outsideParts.Where(x => HasParent(x, BodyPartDefOf.Shoulder)).ToList();
                parts.AddRange(outsideParts.Where(x => x.def == BodyPartDefOf.Leg || HasParent(x, BodyPartDefOf.Leg)));
                parts.RemoveAll(x => x.parts.Any() is false);
                if (parts.Any())
                {
                    var cuts = Rand.Range(1, 6);
                    for (var i = 0; i < cuts; i++)
                    {
                        var part = parts.RandomElement();
                        var damage = new DamageInfo(DamageDefOf.Cut, 1, 1, instigator: pawn, hitPart: part);
                        damage.SetAllowDamagePropagation(false);
                        pawn.TakeDamage(damage);
                    }
                    pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(DefsOf.VAEI_VoidHarmed);
                }
            }
        }

        private static bool HasParent(BodyPartRecord record, BodyPartDef parent)
        {
            if (record.parent != null)
            {
                if (record.parent.def == parent)
                {
                    return true;
                }
                return HasParent(record.parent, parent);
            }
            return false;
        }
    }
}
