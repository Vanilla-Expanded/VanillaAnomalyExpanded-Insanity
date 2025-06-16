using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(InteractionWorker_Slight), "RandomSelectionWeight")]
    public static class InteractionWorker_Slight_RandomSelectionWeight_Patch
    {
        public static void Postfix(Pawn initiator, ref float __result)
        {
            if (initiator.TryGetSanity(out var sanity))
            {
                if (sanity.CurLevel <= 0.75f && sanity.CurLevel > 0.50f)
                {
                    __result *= 2f;
                }
                else if (sanity.CurLevel <= 0.50f && sanity.CurLevel > 0.25f)
                {
                    __result *= 3f;
                }
                else if (sanity.CurLevel <= 0.25f)
                {
                    __result *= 3f;
                }
            }
        }
    }
}
