using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalState_VoidDrawing : MentalState
    {
        private int nextDrawingTick;

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            SetNextDrawingTick();
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            if (pawn.Spawned && pawn.IsHashIntervalTick(30) && Find.TickManager.TicksGame >= nextDrawingTick)
            {
                StartVoidDrawingJob();
            }
        }

        public void SetNextDrawingTick()
        {
            nextDrawingTick = Find.TickManager.TicksGame + Rand.RangeInclusive(600, 1200);
        }

        private void StartVoidDrawingJob()
        {
            if (pawn.CurJobDef != DefsOf.VAEI_VoidDrawingJob && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                var pos = pawn.Position + pawn.Rotation.FacingCell;
                if (GoodCell(pos) is false)
                {
                    pos = GenRadial.RadialCellsAround(pawn.Position, 1.5f, true)
                    .Where(x => GoodCell(x)).FirstOrDefault();
                }
                else if (pos == default)
                {
                    pos = pawn.Position;
                }
                Job voidDrawingJob = JobMaker.MakeJob(DefsOf.VAEI_VoidDrawingJob, pos);
                pawn.jobs.StartJob(voidDrawingJob, JobCondition.InterruptForced);
            }
        }

        private bool GoodCell(IntVec3 x)
        {
            return x.InBounds(pawn.Map) && x.GetFirstBuilding(pawn.Map) is null
                && x.GetFirstItem(pawn.Map) == null  && x != pawn.Position
                && x.Standable(pawn.Map);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextDrawingTick, "nextDrawingTick");
        }
    }
}
