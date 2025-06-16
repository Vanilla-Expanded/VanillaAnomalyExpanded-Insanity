using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(PawnGroupKindWorker), "GeneratePawns", new Type[]
    {
        typeof(PawnGroupMakerParms), typeof(PawnGroupMaker), typeof(bool)
    })]
    public static class PawnGroupKindWorker_GeneratePawns_Patch
    {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result, PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
        {
            foreach (var pawn in GeneratePawns(__result, parms))
            {
                yield return pawn;
            }
        }

        public static IEnumerable<Pawn> GeneratePawns(IEnumerable<Pawn> __result, PawnGroupMakerParms parms)
        {
            var result = __result.ToList();
            if (parms.faction == Faction.OfHoraxCult)
            {
                var joinedCultists = Find.WorldPawns.AllPawnsAlive.Where(x => x.Faction == Faction.OfHoraxCult).ToList();
                foreach (var pawn in joinedCultists)
                {
                    var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(DefsOf.VAEI_JoinedCult);
                    if (hediff != null)
                    {
                        pawn.health.RemoveHediff(hediff);
                        if (result.Contains(pawn) is false)
                        {
                            yield return pawn;
                        }
                    }
                }
            }
            foreach (var r in result)
            {
                yield return r;
            }
        }
    }
}
