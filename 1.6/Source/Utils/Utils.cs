using HarmonyLib;
using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        static Utils()
        {
            new Harmony("VAEInsanityMod").PatchAll();
            foreach (var def in DefDatabase<ThingDef>.AllDefs.Where(x => x.IsCorpse is false))
            {
                if (def.HasComp<CompActivity>())
                {
                    if (VAEInsanityModSettings.suppressingEntities.ContainsKey(def) is false)
                    {
                        VAEInsanityModSettings.suppressingEntities[def] = new SanityEffect(-0.01f);
                    }
                }
                if (def.race != null && def.HasComp<CompStudiable>() && def.race.IsAnomalyEntity
                    || def.HasComp<CompStudyUnlocks>())
                {
                    if (VAEInsanityModSettings.studyingEntities.ContainsKey(def) is false)
                    {
                        VAEInsanityModSettings.studyingEntities[def] = new SanityEffect(-0.01f);
                    }
                }
            }

            foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
            {
                FillEffects(def.killedThingsEffects, VAEInsanityModSettings.killingEntities, ent => ent.thing, ent => new SanityEffect(ent.effect));
                FillEffects(def.disturbingInitiatorEffects, VAEInsanityModSettings.disturbingInitiatorEffects, ent => ent.interaction, ent => new SanityEffect(ent.effect));
                FillEffects(def.interactionEffects, VAEInsanityModSettings.interactionEffects, ent => ent.interaction, ent => new SanityEffect(ent.effect));
                FillEffects(def.nonDisturbingInitiatorEffects, VAEInsanityModSettings.nonDisturbingInitiatorEffects, ent => ent.interaction, ent => new SanityEffect(ent.effect));
                FillEffects(def.hediffEffects, VAEInsanityModSettings.hediffEffects, ent => ent.hediff, ent => new SanityEffect(ent.effect));
                FillEffects(def.usedThingsEffects, VAEInsanityModSettings.usedThingsEffects, ent => ent.thing, ent => new SanityEffect(ent.effect));
                FillEffects(def.ritualEffects, VAEInsanityModSettings.ritualEffects, ent => ent.ritual, ent => new SanityEffect(ent.effect));
                if (def.psychicRitualEffects != null)
                {
                    foreach (var effect in def.psychicRitualEffects)
                    {
                        // For invokerEffect
                        if (effect.invokerEffect != 0)
                        {
                            if (!VAEInsanityModSettings.invokerEffects.ContainsKey(effect.ritual))
                            {
                                VAEInsanityModSettings.invokerEffects[effect.ritual] = new SanityEffect(effect.invokerEffect);
                            }
                        }

                        // For targetEffect
                        if (effect.targetEffect != 0)
                        {
                            if (!VAEInsanityModSettings.targetEffects.ContainsKey(effect.ritual))
                            {
                                VAEInsanityModSettings.targetEffects[effect.ritual] = new SanityEffect(effect.targetEffect);
                            }
                        }

                        // For chanterEffect
                        if (effect.chanterEffect != 0)
                        {
                            if (!VAEInsanityModSettings.chanterEffects.ContainsKey(effect.ritual))
                            {
                                VAEInsanityModSettings.chanterEffects[effect.ritual] = new SanityEffect(effect.chanterEffect);
                            }
                        }
                    }
                }
            }


            void FillEffects<T, TKey>(List<T> effectList, Dictionary<TKey, SanityEffect> targetDict, 
                Func<T, TKey> keySelector, Func<T, SanityEffect> valueSelector) where T : SanityEffectBase
            {
                if (effectList != null)
                {
                    foreach (var effect in effectList)
                    {
                        var key = keySelector(effect);
                        if (!targetDict.ContainsKey(key))
                        {
                            var value = targetDict[key] = valueSelector(effect);
                            if (effect.description.NullOrEmpty() is false)
                            {
                                value.description = effect.description;
                            }
                        }
                    }
                }
            }
        }

        public static bool TryGetEffect<T>(this Dictionary<T, SanityEffect> effectsDict, T key, out SanityEffect effect)
        {
            if (key != null && effectsDict.TryGetValue(key, out effect) && effect.enabled)
            {
                return true;
            }
            effect = null;
            return false;
        }

        public static bool TryGetSanity(this Pawn pawn, out Need_Sanity need)
        {
            need = pawn?.needs?.TryGetNeed<Need_Sanity>();
            return need != null;
        }

        [DebugAction("Pawns", "Sanity +10%", false, false, false, false,false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true, displayPriority = -1000)]
        private static void SanityPlus10(Pawn p)
        {
            if (p.TryGetSanity(out var need))
            {
                need.GainSanity(0.1f, "using DEV: Sanity +10%");
                DebugActionsUtility.DustPuffFrom(p);
            }

        }

        [DebugAction("Pawns", "Sanity -10%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true, displayPriority = -1000)]
        private static void SanityMinus10(Pawn p)
        {
            if (p.TryGetSanity(out var need))
            {
                need.GainSanity(-0.1f, "using DEV: Sanity -10%");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }

        public static void SanityGain(this Pawn pawn, SanityEffect effect, string reason)
        {
            if (pawn.TryGetSanity(out var need))
            {
                if (effect.description.NullOrEmpty() is false)
                {
                    reason = effect.description;
                }
                need.GainSanity(effect.sanityValue.RandomInRange, reason);
            }
        }

        public static void SanityGain(this Pawn pawn, float sanityGain, string reason)
        {
            if (pawn.TryGetSanity(out var need))
            {
                need.GainSanity(sanityGain, reason);
            }
        }

        public static void SanityGainContinuously(this Pawn pawn, float sanityGain, string reason)
        {
            if (pawn.TryGetSanity(out var need))
            {
                var lastRecord = need?.records.LastOrDefault();
                if (lastRecord != null && lastRecord.reason == reason)
                {
                    lastRecord.UpdateRecord(sanityGain);
                }
                else
                {
                    need?.GainSanity(sanityGain, reason, doMessage: false);
                }
            }
        }

        public static bool Wears(this Pawn pawn, ThingDef thingDef)
        {
            return pawn.Wears(thingDef, out _);
        }

        public static bool Wears(this Pawn pawn, ThingDef thingDef, out Apparel apparel)
        {
            apparel = null;
            if (pawn?.apparel?.WornApparel != null)
            {
                foreach (var other in pawn.apparel.WornApparel)
                {
                    if (other.def == thingDef)
                    {
                        apparel = other;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool HasTrait(this Pawn pawn, TraitDef traitDef)
        {
            if (traitDef != null && (pawn?.story?.traits?.HasTrait(traitDef) ?? false))
            {
                return true;
            }
            return false;
        }

        public static HashSet<HediffDef> germaphobeImmuneTo = new HashSet<HediffDef>
        {
            HediffDefOf.Plague, DefsOf.Flu, DefsOf.Malaria
        };

        public static bool CanCatch(this Pawn pawn, HediffDef hediffDef)
        {
            if (pawn.HasTrait(DefsOf.VAEI_Germaphobe) && germaphobeImmuneTo.Contains(hediffDef))
            {
                return false;
            }
            return true;
        }
    }
}
