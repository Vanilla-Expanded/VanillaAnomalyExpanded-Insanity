using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class ThoughtWorker_MoodSwing : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.HasTrait(DefsOf.VAEI_MoodSwings))
            {
                var hediff = p.health.hediffSet.GetFirstHediffOfDef(DefsOf.VAEI_MoodSwing);
                var comp = hediff.TryGetComp<HediffComp_MoodSwingCycle>();
                if (comp != null)
                {
                    return ThoughtState.ActiveAtStage(comp.CurrentStage);
                }
            }
            return ThoughtState.Inactive;
        }
    }
}
