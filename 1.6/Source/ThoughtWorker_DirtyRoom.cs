using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class ThoughtWorker_DirtyRoom : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.Spawned is false || p.HasTrait(DefsOf.VAEI_Germaphobe) is false)
            {
                return ThoughtState.Inactive;
            }
            Room room = p.GetRoom();
            if (room is null || room.ProperRoom is false)
            {
                return ThoughtState.Inactive;
            }
            // Check room cleanliness
            float roomCleanliness = room.GetStat(RoomStatDefOf.Cleanliness);
            if (roomCleanliness < -1.1f) // Very dirty
            {
                return ThoughtState.ActiveAtStage(2);
            }
            else if (roomCleanliness < -0.4f) // Dirty
            {
                return ThoughtState.ActiveAtStage(1);
            }
            else if (roomCleanliness < -0.05f) // Slightly dirty
            {
                return ThoughtState.ActiveAtStage(0);
            }

            return ThoughtState.Inactive;
        }
    }
}
