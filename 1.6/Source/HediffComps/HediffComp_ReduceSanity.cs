using Verse;

namespace VAEInsanity
{
    public class HediffComp_ReduceSanity : HediffComp
    {
        public const int day = 60000;
       
       
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (this.parent.pawn.IsHashIntervalTick(day))
            {
                this.parent.pawn.SanityGain(-0.01f, "VAEI_VoidWeapon".Translate());
                
            }
        }

       
    }
}
