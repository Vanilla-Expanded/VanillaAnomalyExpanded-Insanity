using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(TraitSet), "RemoveTrait")]
    public static class TraitSet_RemoveTrait_Patch
    {
        public static void Postfix(Pawn ___pawn, Trait trait)
        {
            if (___pawn.HasTrait(trait.def) is false)
            {
                if (trait.def == DefsOf.VAEI_Distractable)
                {
                    RemoveHediff(___pawn, DefsOf.VAEI_Focused);
                    RemoveHediff(___pawn, DefsOf.VAEI_Distracted);
                }
                else if (trait.def == DefsOf.VAEI_MoodSwings)
                {
                    RemoveHediff(___pawn, DefsOf.VAEI_MoodSwing);
                }
            }
        }

        public static void RemoveHediff(Pawn pawn, HediffDef def)
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(def);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
