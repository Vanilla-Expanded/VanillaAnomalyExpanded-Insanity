using HarmonyLib;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(HediffSet), "AddDirect")]
    public static class HediffSet_AddDirect_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(HediffSet __instance, Pawn ___pawn, Hediff hediff)
        {
            if (___pawn.CanCatch(hediff.def) is false)
            {
                return false;
            }
            return true;
        }
    }
}
