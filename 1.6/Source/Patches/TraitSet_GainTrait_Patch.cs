using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(TraitSet), "GainTrait")]
    public static class TraitSet_GainTrait_Patch
    {
        public static void Postfix(Pawn ___pawn, Trait trait)
        {
            if (___pawn.HasTrait(trait.def))
            {
                if (trait.def == DefsOf.VAEI_Distractable)
                {
                    var def = Rand.Bool ? DefsOf.VAEI_Focused : DefsOf.VAEI_Distracted;
                    AddHediff(___pawn, def);
                }
                else if (trait.def == DefsOf.VAEI_MoodSwings)
                {
                    AddHediff(___pawn, DefsOf.VAEI_MoodSwing);
                }
            }
        }

        private static void AddHediff(Pawn ___pawn, HediffDef def)
        {
            Hediff hediff = HediffMaker.MakeHediff(def, ___pawn);
            ___pawn.health.AddHediff(hediff);
        }
    }
}
