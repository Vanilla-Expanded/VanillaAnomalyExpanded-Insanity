using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(CompUsable), "UsedBy")]
    public static class CompUsable_UsedBy_Patch
    {
        public static void Postfix(CompUsable __instance, Pawn p)
        {
            if (VAEInsanityModSettings.usedThingsEffects.TryGetEffect(__instance.parent.def, out var effect))
            {
                p.SanityGain(effect, "VAEI_UsingThing".Translate(__instance.parent.def.label));
            }
        }
    }
}
