using HarmonyLib;
using RimWorld;
using System;
using Verse;
using RimWorld.BaseGen;



namespace VanillaRecyclingExpanded
{
    

    [HarmonyPatch(typeof(TerrainDef))]
    [HarmonyPatch("IsCarpet", MethodType.Getter)]
   

    public static class VanillaRecyclingExpanded_TerrainDef_IsCarpet_Patch
    {
        [HarmonyPostfix]
        public static void RemoveFloor(ref TerrainDef __instance, ref bool __result)

        {
            if (__instance == InternalDefOf.VRecyclingE_TrashBrickFloor)
            {
                
                    __result = true;
                
            }



        }

    }


}