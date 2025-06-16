using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class JobGiver_ViciousMockery : ThinkNode_JobGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            if (!(pawn.MentalState is MentalState_InsultingSpree { target: not null } mentalState_InsultingSpree) 
                || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly))
            {
                return null;
            }
            if (!SocialInteractionUtility.BestInteractableCell(pawn, mentalState_InsultingSpree.target).IsValid)
            {
                return null;
            }
            return JobMaker.MakeJob(DefsOf.VAEI_ViciousMockJob, mentalState_InsultingSpree.target);
        }
    }
}
