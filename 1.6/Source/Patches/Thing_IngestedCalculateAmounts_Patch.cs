using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Thing), "IngestedCalculateAmounts")]
    public static class Thing_IngestedCalculateAmounts_Patch
    {
        public static void Postfix(Thing __instance, Pawn ingester, ref int numTaken)
        {
            if (VAEInsanityModSettings.twistedMeatValue.TryGetEffect(out var effect))
            {
                if (__instance.def == ThingDefOf.Meat_Twisted)
                {
                    float sanityChange = -(numTaken / 80.0f) * -effect;
                    ingester.SanityGain(sanityChange, "VEAI_EatingTwistedMeat".Translate());
                }
                else if (__instance.TryGetComp<CompIngredients>() is CompIngredients compIngredients && compIngredients.ingredients.Contains(ThingDefOf.Meat_Twisted))
                {
                    ingester.SanityGain(effect, "VEAI_EatingTwistedMeatAsIngredient".Translate());
                }
            }
        }
    }
}
