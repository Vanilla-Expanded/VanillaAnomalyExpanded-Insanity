using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "SuccessChance")]
    public static class InteractionWorker_RomanceAttempt_SuccessChance_Patch
    {
        public static void Postfix(Pawn initiator, Pawn recipient, ref float __result)
        {
            if (initiator.HasTrait(DefsOf.VAEI_Noncommittal) || recipient.HasTrait(DefsOf.VAEI_Noncommittal))
            {
                __result = 0f;
            }
        }
    }
}
