using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch]
    public static class PawnObserver_ObserveSurroundingThings_Patch
    {
        public static MethodBase TargetMethod()
        {
            var methods = typeof(PawnObserver).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            MethodInfo targetMethod = methods.FirstOrDefault(m =>
                m.Name.Contains("<ObserveSurroundingThings>") && // Ensures the method is related to ObserveSurroundingThings
                m.GetParameters().Length == 1 && // Ensures there is only one parameter
                m.GetParameters()[0].ParameterType == typeof(Region)); // Checks if the parameter type is Region
            return targetMethod;
        }

        public static void Postfix(PawnObserver __instance, Region reg)
        {
            foreach (var item in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Filth).OfType<VoidDrawing>())
            {
                if (__instance.PossibleToObserve(item))
                {
                    Thought_Memory thought_Memory = item.GiveObservedThought(__instance.pawn);
                    if (thought_Memory != null)
                    {
                        __instance.pawn.needs?.mood?.thoughts?.memories.TryGainMemory(thought_Memory);
                    }
                }
            }
        }
    }
}
