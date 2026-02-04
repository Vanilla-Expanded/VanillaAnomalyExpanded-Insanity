using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalState_JoinCult : MentalState
    {
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}
