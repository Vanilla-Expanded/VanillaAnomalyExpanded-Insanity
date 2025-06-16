using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(CompStudiable), "Study")]
    public static class CompStudiable_Study_Patch
    {
        public static void Postfix(CompStudiable __instance, Pawn studier, float anomalyKnowledgeAmount)
        {
            if (VAEInsanityModSettings.studyingEntities.TryGetEffect(__instance.parent.def, out var effect))
            {
                float sanityChange = anomalyKnowledgeAmount * effect.sanityValue.max;
                studier.SanityGain(sanityChange, "VEAI_AnomalyStudy".Translate(__instance.parent.Label));
            }
        }
    }
}
