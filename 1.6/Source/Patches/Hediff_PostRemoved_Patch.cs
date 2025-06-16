using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Hediff), "PostRemoved")]
    public static class Hediff_PostRemoved_Patch
    {
        public static void Postfix(Hediff __instance)
        {
            if (__instance.pawn.RaceProps.Humanlike)
            {
                CheckAndRemoveTrait(__instance, HediffDefOf.Inhumanized, DefsOf.VAEI_Inhumanized);
                CheckAndRemoveTrait(__instance, HediffDefOf.VoidTouched, DefsOf.VAEI_VoidTouched);
            }
        }

        private static void CheckAndRemoveTrait(Hediff hediff, HediffDef associatedHediffDef, TraitDef traitToRemove)
        {
            if (hediff.def == associatedHediffDef && !hediff.pawn.health.hediffSet.hediffs.Any(h => h.def == associatedHediffDef))
            {
                var trait = hediff.pawn.story.traits.GetTrait(traitToRemove);
                if (trait != null)
                {
                    hediff.pawn.story.traits.RemoveTrait(trait);
                }
            }
        }
    }
}
