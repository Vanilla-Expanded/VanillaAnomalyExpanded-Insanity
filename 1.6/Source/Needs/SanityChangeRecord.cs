using Verse;

namespace VAEInsanity
{
    public class SanityChangeRecord : IExposable
    {
        public float effect;
        public int ticksOccured;
        public string reason;
        public SanityChangeRecord Copy()
        {
            return new SanityChangeRecord
            {
                effect = effect,
                ticksOccured = ticksOccured,
                reason = reason
            };
        }

        public void UpdateRecord(float effect)
        {
            this.effect += effect;
            ticksOccured = Find.TickManager.TicksGame;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref effect, "effect");
            Scribe_Values.Look(ref ticksOccured, "ticksOccured");
            Scribe_Values.Look(ref reason, "reason");
        }
    }
}
