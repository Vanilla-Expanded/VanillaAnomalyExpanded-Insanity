using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(JobDriver_ActivitySuppression), "TrySuppress")]
    public static class JobDriver_ActivitySuppression_TrySuppress_Patch
    {
        public static void Postfix(ref Toil __result, JobDriver_ActivitySuppression __instance)
        {
            __result.AddPreTickAction(delegate
            {
                if (VAEInsanityModSettings.suppressingEntities.TryGetEffect(__instance.ThingToSuppress.def, 
                    out var effect))
                {
                    __instance.pawn.SanityGainContinuously(effect.sanityValue.max / GenDate.TicksPerDay, "VAEI_SuppressingEntity".Translate(__instance.ThingToSuppress.Label));
                }
            });
        }
    }
}
