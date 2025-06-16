using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static Verse.PawnCapacityUtility;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculateCapacityLevel")]
    public static class PawnCapacityUtility_CalculateCapacityLevel_Patch
    {
        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors)
        {
            if (capacity == PawnCapacityDefOf.Talking && diffSet.pawn.Wears(DefsOf.VAEI_Apparel_Gag))
            {
                __result = 0f;
                impactors?.Add(new CapacityImpactorApparel { 
                    apparel = DefsOf.VAEI_Apparel_Gag,
                    capacity = capacity,
                });
            }
            else if (capacity == PawnCapacityDefOf.Manipulation && diffSet.pawn.Wears(DefsOf.VAEI_Apparel_StraitJacket))
            {
                __result = 0f;
                impactors?.Add(new CapacityImpactorApparel
                {
                    apparel = DefsOf.VAEI_Apparel_StraitJacket,
                    capacity = capacity,
                });
            }
        }
    }

    public class CapacityImpactorApparel : CapacityImpactorCapacity
    {
        public ThingDef apparel;

        public override string Readable(Pawn pawn)
        {
            return "VAEI_Wearing".Translate(apparel.label);
        }
    }
}
