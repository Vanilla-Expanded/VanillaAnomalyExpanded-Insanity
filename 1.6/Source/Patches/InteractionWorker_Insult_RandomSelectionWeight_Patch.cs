using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(InteractionWorker_Insult), "RandomSelectionWeight")]
    public static class InteractionWorker_Insult_RandomSelectionWeight_Patch
    {
        public static void Postfix(Pawn initiator, ref float __result)
        {
            if (initiator.TryGetSanity(out var sanity))
            {
                if (sanity.CurLevel <= 0.75f && sanity.CurLevel > 0.50f)
                {
                    __result *= 1.5f;
                }
                else if (sanity.CurLevel <= 0.50f && sanity.CurLevel > 0.25f)
                {
                    __result *= 2f;
                }
                else if (sanity.CurLevel <= 0.25f)
                {
                    __result *= 2f;
                }
            }
        }
    }
}
