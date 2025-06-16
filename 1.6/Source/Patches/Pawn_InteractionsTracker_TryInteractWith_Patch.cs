using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker), "TryInteractWith")]
    public static class Pawn_InteractionsTracker_TryInteractWith_Patch
    {
        public static void Prefix(Pawn_InteractionsTracker __instance, ref InteractionDef intDef)
        {
            if (__instance.pawn.TryGetSanity(out var sanity) is false)
            {
                return;
            }

            float sanityLevel = sanity.CurLevel;
            if (sanityLevel > 0.75f)
            {
                return;
            }
            else if (sanityLevel > 0.50f)
            {
                if (intDef == InteractionDefOf.Chitchat && Rand.Value < 0.1f)
                {
                    intDef = DefsOf.DisturbingChat;
                }
                else if (intDef == DefsOf.KindWords && Rand.Value < 0.25f)
                {
                    intDef = DefsOf.VAEI_TwistedWords;
                }
            }
            else if (sanityLevel > 0.25f)
            {
                if (intDef == InteractionDefOf.Chitchat && Rand.Value < 0.25f)
                {
                    intDef = DefsOf.DisturbingChat;
                }
                else if (intDef == DefsOf.KindWords && Rand.Value < 0.5f)
                {
                    intDef = DefsOf.VAEI_TwistedWords;
                }
                else if (intDef == InteractionDefOf.DeepTalk && Rand.Value < 0.5f)
                {
                    intDef = DefsOf.VAEI_UnsettlingTalk;
                }
            }
            else if (sanityLevel >= 0.0f)
            {
                if (intDef == InteractionDefOf.Chitchat && Rand.Value < 0.5f)
                {
                    intDef = DefsOf.DisturbingChat;
                }
                else if (intDef == DefsOf.KindWords)
                {
                    intDef = DefsOf.VAEI_TwistedWords;
                }
                else if (intDef == InteractionDefOf.DeepTalk && Rand.Value < 0.75f)
                {
                    intDef = DefsOf.VAEI_UnsettlingTalk;
                }
            }
        }

        public static void Postfix(Pawn_InteractionsTracker __instance, bool __result, Pawn recipient,
            InteractionDef intDef)
        {
            if (VAEInsanityModSettings.interactionEffects.TryGetEffect(intDef, out var effect))
            {
                recipient.SanityGain(effect, "VAEI_Interaction".Translate(intDef.label, __instance.pawn.Named("PAWN")));
            }
            if (__instance.pawn.story?.IsDisturbing==true)
            {
                if (VAEInsanityModSettings.disturbingInitiatorEffects.TryGetEffect(intDef, out var disturbing))
                {
                    recipient.SanityGain(disturbing, "VAEI_DisturbingInteraction".Translate(intDef.label, __instance.pawn.Named("PAWN")));

                }
            }
            else if (VAEInsanityModSettings.nonDisturbingInitiatorEffects.TryGetEffect(intDef, out var nonDisturbing))
            {
                recipient.SanityGain(nonDisturbing, "VAEI_Interaction".Translate(intDef.label, __instance.pawn.Named("PAWN")));
            }
        }
    }
}
