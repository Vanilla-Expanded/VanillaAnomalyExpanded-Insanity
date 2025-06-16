using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    public class PsychicRitualEffect
    {
        public PsychicRitualDef_InvocationCircle ritual;
        public float invokerEffect, targetEffect, chanterEffect;
    }

    public class RitualEffect : SanityEffectBase
    {
        public RitualOutcomeEffectDef ritual;
    }

    public abstract class SanityEffectBase
    {
        public FloatRange effect;
        public string description;
    }

    public class InteractionEffect : SanityEffectBase
    {
        public InteractionDef interaction;
    }

    public class HediffEffect : SanityEffectBase
    {
        public HediffDef hediff;
    }
    public class ThingEffect : SanityEffectBase
    {
        public ThingDef thing;
    }
    public class SanityEffectsDef : Def
    {
        public List<PsychicRitualEffect> psychicRitualEffects;
        public List<InteractionEffect> disturbingInitiatorEffects, interactionEffects, nonDisturbingInitiatorEffects;
        public List<HediffEffect> hediffEffects;
        public List<ThingEffect> usedThingsEffects, killedThingsEffects;
        public List<RitualEffect> ritualEffects;
        public List<TraitDef> rehumanizationTraits;
    }
}
