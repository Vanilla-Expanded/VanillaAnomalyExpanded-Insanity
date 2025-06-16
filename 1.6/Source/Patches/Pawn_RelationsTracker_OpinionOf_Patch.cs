using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "OpinionOf")]
    public static class Pawn_RelationsTracker_OpinionOf_Patch
    {
        public static void Postfix(Pawn_RelationsTracker __instance, ref int __result)
        {
            if (__instance.pawn.HasTrait(DefsOf.VAEI_Noncommittal))
            {
                __result = Mathf.Min(25, __result);
            }
        }
    }
}
