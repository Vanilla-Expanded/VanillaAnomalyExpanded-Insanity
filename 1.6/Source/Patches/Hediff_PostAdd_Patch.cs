using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Hediff), "PostAdd")]
    public static class Hediff_PostAdd_Patch
    {
        public static void Postfix(Hediff __instance, DamageInfo? dinfo)
        {
            if (__instance.pawn.RaceProps.Humanlike && __instance.pawn.TryGetSanity(out var sanity))
            {
                CheckAndAddTrait(__instance, HediffDefOf.Inhumanized, DefsOf.VAEI_Inhumanized);
                CheckAndAddTrait(__instance, HediffDefOf.VoidTouched, DefsOf.VAEI_VoidTouched);
                if (sanity.rehumanizedTrait != null)
                {
                    var trait = __instance.pawn.story.traits.GetTrait(sanity.rehumanizedTrait);
                    if (trait != null)
                    {
                        __instance.pawn.story.traits.RemoveTrait(trait);
                    }
                }
            }
        }

        private static void CheckAndAddTrait(Hediff hediff, HediffDef requiredHediffDef, TraitDef traitToAdd)
        {
            if (hediff.def == requiredHediffDef && !hediff.pawn.HasTrait(traitToAdd))
            {
                hediff.pawn.story.traits.GainTrait(new Trait(traitToAdd));
            }
        }
    }
}
