using HarmonyLib;
using RimWorld;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Wear")]
    public static class Pawn_ApparelTracker_Wear_Patch
    {
        public static void Postfix(Pawn_ApparelTracker __instance, Apparel newApparel)
        {
            if (newApparel?.def == DefsOf.VAEI_Apparel_StraitJacket)
            {
                __instance.pawn.health.CheckForStateChange(null, null);
            }
        }
    }
}
