using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class JobDriver_VoidDrawing : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(base.TargetA, job, 1, -1, null, errorOnFailed);
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => pawn.MentalState is not MentalState_VoidDrawing);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            toil.initAction = delegate
            {
                pawn.jobs.posture = PawnPosture.Standing;
            };
            toil.handlingFacing = true;
            toil.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(base.TargetA.Cell);
            };
            toil.AddFinishAction(delegate
            {
                FilthMaker.TryMakeFilth(base.TargetA.Cell, pawn.Map, DefsOf.VAEI_VoidDrawing);
                var state = pawn.MentalState as MentalState_VoidDrawing;
                if (state != null)
                {
                    state.SetNextDrawingTick();
                }
            });
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 300;
            yield return toil;
        }
    }
}
