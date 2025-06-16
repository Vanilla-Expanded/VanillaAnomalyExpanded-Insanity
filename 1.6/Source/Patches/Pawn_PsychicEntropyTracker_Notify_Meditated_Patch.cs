using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "Notify_Meditated")]
    public static class Pawn_PsychicEntropyTracker_Notify_Meditated_Patch
    {
        public static void Postfix(Pawn_PsychicEntropyTracker __instance)
        {
            if (VAEInsanityModSettings.meditatingValue.TryGetEffect(out var effect))
            {
                __instance.pawn.SanityGainContinuously(effect / GenDate.TicksPerDay, "VAEI_Meditating".Translate());
            }
        }
    }
}
