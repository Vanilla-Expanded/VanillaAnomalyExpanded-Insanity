using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(PawnGroupMakerUtility), "GeneratePawns")]
    public static class PawnGroupMakerUtility_GeneratePawns
    {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result,
            PawnGroupMakerParms parms, bool warnOnZeroResults)
        {
            if (parms.faction == Faction.OfHoraxCult)
            {
                foreach (var pawn in PawnGroupKindWorker_GeneratePawns_Patch.GeneratePawns(__result, parms))
                {
                    yield return pawn;
                }
            }
            else
            {
                foreach (var r in __result)
                {
                    yield return r;
                }
            }
        }
    }
}
