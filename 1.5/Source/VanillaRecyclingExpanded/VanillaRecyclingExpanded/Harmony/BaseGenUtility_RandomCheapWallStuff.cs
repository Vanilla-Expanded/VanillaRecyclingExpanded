using HarmonyLib;
using RimWorld;
using System;
using Verse;
using RimWorld.BaseGen;



namespace VanillaRecyclingExpanded
{


    [HarmonyPatch(typeof(GenStuff))]
    [HarmonyPatch("RandomStuffInexpensiveFor")]
    [HarmonyPatch(new Type[] { typeof(ThingDef), typeof(TechLevel), typeof(Predicate<ThingDef>) })]
   

    public static class VanillaRecyclingExpanded_GenStuff_RandomStuffInexpensiveFor_Patch
    {
        [HarmonyPostfix]
        public static void RemoveMaterial(ref ThingDef __result)

        {
            if (__result != null)
            {
                if (__result == InternalDefOf.VRecyclingE_TrashBrick)
                {

                    __result = ThingDefOf.BlocksGranite;
                }
            }



        }

    }


}