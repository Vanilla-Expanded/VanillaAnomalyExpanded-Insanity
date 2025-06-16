using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(LordToil_PsychicRitual), "RitualCompleted")]
    public static class LordToil_PsychicRitual_RitualCompleted_Patch
    {
        public static void Prefix(LordToil_PsychicRitual __instance)
        {
            if (__instance.def is PsychicRitualDef_InvocationCircle ritual)
            {
                if (VAEInsanityModSettings.invokerEffects.TryGetEffect(ritual, out var effect))
                {
                    var invoker = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(ritual.InvokerRole);
                    invoker.SanityGain(effect, "VAEI_InvokerEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                }
                if (VAEInsanityModSettings.targetEffects.TryGetEffect(ritual, out var effect2))
                {
                    var target = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(ritual.TargetRole);
                    target.SanityGain(effect2, "VAEI_TargetEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                }
                if (VAEInsanityModSettings.chanterEffects.TryGetEffect(ritual, out var effect3))
                {
                    var chanter = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(ritual.ChanterRole);
                    chanter.SanityGain(effect3, "VAEI_ChanterEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                }
            }
        }
    }
}
