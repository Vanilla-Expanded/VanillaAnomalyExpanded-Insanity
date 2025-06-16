using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(ReadingOutcomeDoerGainAnomalyResearch), "OnReadingTick")]
    public static class ReadingOutcomeDoerGainAnomalyResearch_OnReadingTick_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var modifyMethod = AccessTools.Method(typeof(ReadingOutcomeDoerGainAnomalyResearch_OnReadingTick_Patch), 
                nameof(ModifyKnowledgeValue));

            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (codes[i].opcode == OpCodes.Mul)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, modifyMethod);
                }
            }
        }

        public static float ModifyKnowledgeValue(float originalValue, Pawn reader)
        {
            if (reader.TryGetSanity(out var sanity) is false)
            {
                return originalValue;
            }
            if (sanity.CurLevel > 0.75f)
            {
                return originalValue;
            }
            else if (sanity.CurLevel > 0.50f)
            {
                return originalValue + (0.07f / GenDate.TicksPerHour);
            }
            else if (sanity.CurLevel > 0.25f)
            {
                return originalValue + (0.15f / GenDate.TicksPerHour);
            }
            else if (sanity.CurLevel > 0.0f)
            {
                return originalValue + (0.23f / GenDate.TicksPerHour);
            }
            return originalValue;
        }

        public static void Postfix(Pawn reader)
        {
            if (VAEInsanityModSettings.readingTomeValue.TryGetEffect(out var effect))
            {
                reader.SanityGainContinuously(effect / GenDate.TicksPerDay, "VAEI_ReadingTome".Translate());
            }
        }
    }
}
