using Verse;

namespace VAEInsanity
{
    public class SanityEffect : IExposable
    {
        public bool isSingleSlider;
        public string description;
        public FloatRange sanityValue;
        public bool enabled = true;
        public SanityEffect()
        {

        }

        public SanityEffect(FloatRange value)
        {
            this.sanityValue = value;
            if (value.min == value.max)
            {
                isSingleSlider = true;
            }
        }

        public SanityEffect(float value)
        {
            this.sanityValue = new FloatRange(value);
            isSingleSlider = true;
        }

        public bool TryGetEffect(out float effect)
        {
            if (enabled)
            {
                effect = sanityValue.RandomInRange;
                return true;
            }
            effect = 0f;
            return false;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref sanityValue, "sanityValue");
            Scribe_Values.Look(ref description, "description");
            Scribe_Values.Look(ref isSingleSlider, "isSingleSlider");
            Scribe_Values.Look(ref enabled, "enabled", true);
        }
    }
}
