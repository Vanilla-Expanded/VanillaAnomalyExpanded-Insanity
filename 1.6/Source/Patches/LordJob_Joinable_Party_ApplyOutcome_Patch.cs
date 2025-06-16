using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(LordJob_Joinable_Party), "ApplyOutcome")]
    public static class LordJob_Joinable_Party_ApplyOutcome_Patch
    {
        public static void Prefix(LordJob_Joinable_Party __instance, LordToil_Party toil)
        {
            List<Pawn> ownedPawns = __instance.lord.ownedPawns;
            LordToilData_Gathering lordToilData_Gathering = (LordToilData_Gathering)toil.data;

            for (int i = 0; i < ownedPawns.Count; i++)
            {
                Pawn pawn = ownedPawns[i];
                if (lordToilData_Gathering.presentForTicks.TryGetValue(pawn, out var value) && value > 0)
                {
                    pawn.SanityGain(VAEInsanityModSettings.partyValue, "VAEI_Attending".Translate(__instance.gatheringDef.label));
                }
            }
        }
    }
}
