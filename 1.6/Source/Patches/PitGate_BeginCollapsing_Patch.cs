using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(PitGate), "BeginCollapsing")]
    public static class PitGate_BeginCollapsing_Patch
    {
        public static void Postfix(PitGate __instance)
        {
            foreach (var pawn in __instance.Map.mapPawns.AllHumanlike)
            {
                if (VAEInsanityModSettings.pitGateCollapsing.TryGetEffect(out var effect))
                {
                    pawn.SanityGain(effect, "VAEI_PitgateCollapsed".Translate());
                }
            }
        }
    }
}
