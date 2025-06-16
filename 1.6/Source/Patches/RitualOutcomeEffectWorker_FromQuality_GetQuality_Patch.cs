using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality), "GetQuality")]
    public static class RitualOutcomeEffectWorker_FromQuality_GetQuality_Patch
    {
        public static void Postfix(float __result, RitualOutcomeEffectWorker_FromQuality __instance, LordJob_Ritual jobRitual, float progress)
        {
            if (progress > 0)
            {
                if (VAEInsanityModSettings.ritualEffects.TryGetEffect(__instance.def, out var effect))
                {
                    foreach (var pawn in jobRitual.totalPresenceTmp.Keys)
                    {
                        var gain = Mathf.Lerp(effect.sanityValue.min, effect.sanityValue.max, __result);
                        pawn.SanityGain(gain, "VAEI_Attending".Translate(jobRitual.Ritual.name));
                    }
                }
            }
        }
    }
}
