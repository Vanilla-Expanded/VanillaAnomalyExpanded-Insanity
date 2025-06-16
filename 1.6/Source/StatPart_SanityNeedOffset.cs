using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace VAEInsanity
{
    [HotSwappable]
    public class StatPart_SanityNeedOffset : StatPart
    {

        public override void TransformValue(StatRequest req, ref float val)
        {
            TryGetSanityValue(req.Thing as Pawn, out val, out _);
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (TryGetSanityValue(req.Thing as Pawn, out var val, out var explanation))
            {
                var part = "VAEI_StatsReport_SanityGainPerDay".Translate(val.ToStringWithSign());
                if (explanation.Length > 0)
                {
                    part += "\n" + "VAEI_PassiveEffects".Translate(explanation);
                }
                return part;
            }
            return null;
        }
        public static bool TryGetSanityValue(Pawn pawn, out float val, out StringBuilder explanation)
        {
            explanation = new StringBuilder();
            val = 0f;
            if (pawn.TryGetSanity(out var sanity))
            {
                var currentSanityLevel = sanity.CurLevel;

                // Handle low and high sanity effects
                if (currentSanityLevel <= 0.1f && VAEInsanityModSettings.lowSanityValue.TryGetEffect(out var lowSanityEffect))
                {
                    sanity.AddEffect(ref val, lowSanityEffect, explanation, "VAEI_LowSanity".Translate());
                }
                else if (currentSanityLevel >= 0.9f && VAEInsanityModSettings.highSanityValue.TryGetEffect(out var highSanityEffect))
                {
                    sanity.AddEffect(ref val, highSanityEffect, explanation, "VAEI_HighSanity".Translate());
                }

                // Handle hediff-related sanity effects
                foreach (var hediff in pawn.health.hediffSet.hediffs)
                {
                    if (VAEInsanityModSettings.hediffEffects.TryGetEffect(hediff.def, out var effect))
                    {
                        var value = effect.sanityValue.RandomInRange;
                        sanity.AddEffect(ref val, value, explanation, "VAEI_AnomalyBodypart".Translate(hediff.Label));
                    }
                }

                // Handle duplication-related sanity effects
                if (pawn.duplicate.duplicateOf != int.MinValue)
                {
                    var comp = Current.Game.GetComponent<GameComponent_PawnDuplicator>();
                    if (comp.duplicates.TryGetValue(pawn.duplicate.duplicateOf, out var duplicates))
                    {
                        if (duplicates.pawns.Any(x => x.Dead is false && x != pawn))
                        {
                            if (VAEInsanityModSettings.duplicateSanityEffect.TryGetEffect(out var duplicateEffect))
                            {
                                sanity.AddEffect(ref val, duplicateEffect, explanation, "VAEI_BeingDuplicated".Translate());
                            }
                        }
                    }
                }

                // Handle unnatural corpse-related sanity effects
                if (Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(pawn, out var _))
                {
                    if (VAEInsanityModSettings.unnaturalCorpseSanityEffect.TryGetEffect(out var unnaturalCorpseEffect))
                    {
                        sanity.AddEffect(ref val, unnaturalCorpseEffect, explanation, "VAEI_UnnaturalCorpse".Translate());
                    }
                }

                // Handle map-related sanity effects
                if (pawn.Spawned)
                {
                    // Handle labyrinth effect
                    if (pawn.Map.generatorDef == MapGeneratorDefOf.Labyrinth)
                    {
                        if (VAEInsanityModSettings.labyrinthSanityEffect.TryGetEffect(out var labyrinthEffect))
                        {
                            sanity.AddEffect(ref val, labyrinthEffect, explanation, "VAEI_BeingInLabyrinth".Translate());
                        }
                    }

                    // Handle unnatural darkness effect
                    if (GameCondition_UnnaturalDarkness.InUnnaturalDarkness(pawn))
                    {
                        if (VAEInsanityModSettings.unnaturalDarknessSanityEffect.TryGetEffect(out var unnaturalDarknessEffect))
                        {
                            sanity.AddEffect(ref val, unnaturalDarknessEffect, explanation, "VAEI_BeingInUnnaturalDarkness".Translate());
                        }
                    }
                }
            }

            return explanation.Length > 0;
        }
    }
}
