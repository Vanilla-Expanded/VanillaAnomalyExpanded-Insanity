using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalBreakWorker_VoidDrawing : MentalBreakWorker
    {
        public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
        {
            if (pawn.TryGetSanity(out var sanity) is false)
            {
                return 0f;
            }
            float sanityLevel = sanity.CurLevel;
            if (sanityLevel > 0.75f)
            {
                return 0f;
            }
            else if (sanityLevel > 0.50f)
            {
                return 1f;
            }
            return 1.5f;
        }
    }
}
