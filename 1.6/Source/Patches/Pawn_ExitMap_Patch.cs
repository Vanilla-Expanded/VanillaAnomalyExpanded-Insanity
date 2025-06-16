using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn), "ExitMap")]
    public static class Pawn_ExitMap_Patch
    {
        public static void Prefix(Pawn __instance)
        {
            var state = __instance.MentalState as MentalState_JoinCult;
            if (state != null && __instance.Faction != Faction.OfHoraxCult)
            {
                __instance.SetFaction(Faction.OfHoraxCult);
                __instance.health.AddHediff(DefsOf.VAEI_JoinedCult);
            }
        }
    }
}
