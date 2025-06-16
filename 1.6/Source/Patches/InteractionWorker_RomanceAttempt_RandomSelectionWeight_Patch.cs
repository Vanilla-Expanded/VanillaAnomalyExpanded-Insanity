using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight_Patch
    {
        public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
        {
            if (initiator.HasTrait(DefsOf.VAEI_Noncommittal))
            {
                __result = 0f;
            }
            else if (recipient.HasTrait(DefsOf.VAEI_Noncommittal))
            {
                __result = 0f;
            }
        }
    }
}
