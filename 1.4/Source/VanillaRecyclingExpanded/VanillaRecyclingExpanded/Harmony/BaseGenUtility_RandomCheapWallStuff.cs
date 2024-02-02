using HarmonyLib;
using RimWorld;
using System;
using Verse;
using RimWorld.BaseGen;



namespace VanillaRecyclingExpanded
{


    [HarmonyPatch(typeof(BaseGenUtility))]
    [HarmonyPatch("RandomCheapWallStuff")]
    [HarmonyPatch(new Type[] { typeof(TechLevel), typeof(bool) })]


    public static class VanillaRecyclingExpanded_BaseGenUtility_RandomCheapWallStuff_Patch
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