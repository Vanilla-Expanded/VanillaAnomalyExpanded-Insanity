using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalBreakWorker_JoinCult : MentalBreakWorker
    {
        public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
        {
            if (pawn.Inhumanized() is false || PawnsFinder.AllMaps_FreeColonists.Count < 12)
            {
                return 0;
            }
            return 1f;
        }

        public override bool BreakCanOccur(Pawn pawn)
        {
            if (pawn.Inhumanized() is false || PawnsFinder.AllMaps_FreeColonists.Count < 12)
            {
                return false;
            }
            return base.BreakCanOccur(pawn);
        }
    }
}
