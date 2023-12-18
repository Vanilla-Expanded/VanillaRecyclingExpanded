using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace VanillaRecyclingExpanded
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.VERecycling");
            harmony.PatchAll(Assembly.GetExecutingAssembly());       
        }          
    }
}
