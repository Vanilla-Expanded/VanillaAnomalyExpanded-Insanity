using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class StatPart_Sanity : StatPart
    {
        public List<SanityFactor> factors;
        public List<SanityOffset> offsets;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.HasThing && req.Thing is Pawn pawn && pawn.TryGetSanity(out var sanity))
            {
                float factor = GetFactor(sanity.CurLevel);
                val *= factor;
                val += GetOffset(pawn, sanity.CurLevel);
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && req.Thing is Pawn pawn && pawn.TryGetSanity(out var sanity))
            {
                float curLevel = sanity.CurLevel;
                float factor = GetFactor(curLevel);
                float offset = GetOffset(pawn, curLevel);
                string explanation = "";

                if (factor != 1f)
                {
                    explanation += "VAEI_SanityFactorExplanation".Translate(curLevel.ToStringPercent(),
                        factor.ToStringByStyle(parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Factor));
                }

                if (offset != 0f)
                {
                    if (!string.IsNullOrEmpty(explanation))
                    {
                        explanation += "\n";
                    }
                    explanation += "VAEI_SanityOffsetExplanation".Translate(curLevel.ToStringPercent(),
                        offset.ToStringByStyle(parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Offset));
                }
                return explanation;
            }
            return null;
        }

        private float GetFactor(float sanityLevel)
        {
            if (factors is not null)
            {
                foreach (SanityFactor factor in factors)
                {
                    if (sanityLevel >= factor.min && sanityLevel < factor.max)
                    {
                        return factor.factor;
                    }
                }
            }
            return 1f;
        }

        private float GetOffset(Pawn pawn, float sanityLevel)
        {
            if (offsets is not null)
            {
                foreach (SanityOffset offset in offsets)
                {
                    if (pawn.HasTrait(DefsOf.VAEI_Inhumanized) == offset.inhumanized
                        && sanityLevel >= offset.min && sanityLevel < offset.max)
                    {
                        return offset.offset;
                    }
                }
            }
            return 0f;
        }
    }

    public class SanityFactor
    {
        public float min;
        public float max;
        public float factor;
    }

    public class SanityOffset
    {
        public float min;
        public float max;
        public float offset;
        public bool inhumanized;
    }
}
