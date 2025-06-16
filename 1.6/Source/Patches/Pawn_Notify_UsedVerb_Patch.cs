using HarmonyLib;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn), "Notify_UsedVerb")]
    public static class Pawn_Notify_UsedVerb_Patch
    {
        public static void Postfix(Pawn pawn, Verb verb)
        {
            if (VAEInsanityModSettings.usedThingsEffects.TryGetEffect(verb.EquipmentSource?.def, out var thingEffect))
            {
                pawn.SanityGain(thingEffect, "VAEI_UsingThing".Translate(verb.EquipmentSource?.def.label));
            }
        }
    }
}
