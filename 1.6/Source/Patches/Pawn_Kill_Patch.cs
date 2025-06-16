using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Pawn_Kill_Patch
    {
        public static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (dinfo.HasValue && dinfo.Value.Instigator is Pawn instigator)
            {
                if (__instance.IsShambler && instigator.TryGetSanity(out var need))
                {
                    if (need.killedShamblers.Contains(__instance) is false
                        && VAEInsanityModSettings.killingShamblerValue.TryGetEffect(out var effect))
                    {
                        need.killedShamblers.Add(__instance);
                        need.GainSanity(effect, "VAEI_KillingThing".Translate(MutantDefOf.Shambler.label));
                    }
                }
                else if (__instance.kindDef == PawnKindDefOf.Nociosphere && instigator.Faction is not null)
                {
                    foreach (var pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(instigator.Faction))
                    {
                        if (VAEInsanityModSettings.killingNociosphereValue.TryGetEffect(out var effect))
                        {
                            pawn.SanityGain(effect, "VAEI_WeKilledNociosphere".Translate());
                        }
                    }
                }
                else if (VAEInsanityModSettings.killingEntities.TryGetEffect(__instance.def, out var effect))
                {
                    instigator.SanityGain(effect, "VAEI_KillingThing".Translate(__instance.def.label));
                }
            }
        }
    }
}
