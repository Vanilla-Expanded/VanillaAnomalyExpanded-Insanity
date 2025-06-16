using HarmonyLib;
using RimWorld;
using RimWorld.Utility;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(VoidAwakeningUtility), "DisruptTheLink")]
    public static class VoidAwakeningUtility_DisruptTheLink_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            foreach (var pawn2 in PawnsFinder.AllMaps_SpawnedPawnsInFaction(pawn.Faction))
            {
                if (VAEInsanityModSettings.voidClosing.TryGetEffect(out var effect))
                {
                    pawn2.SanityGain(effect, "VAEI_VoidClosing".Translate());
                }
            }
        }
    }
}
