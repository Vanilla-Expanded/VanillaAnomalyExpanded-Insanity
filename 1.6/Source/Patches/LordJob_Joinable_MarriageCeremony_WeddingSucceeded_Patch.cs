using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(LordJob_Joinable_MarriageCeremony), "WeddingSucceeded")]
    public static class LordJob_Joinable_MarriageCeremony_WeddingSucceeded_Patch
    {
        public static void Prefix(LordJob_Joinable_MarriageCeremony __instance)
        {
            List<Pawn> ownedPawns = __instance.lord.ownedPawns;
            for (int i = 0; i < ownedPawns.Count; i++)
            {
                Pawn pawn = ownedPawns[i];
                pawn.SanityGain(VAEInsanityModSettings.marriageCeremonyValue, "VAEI_Attending".Translate(GatheringDefOf.MarriageCeremony.label));
            }
        }
    }
}
