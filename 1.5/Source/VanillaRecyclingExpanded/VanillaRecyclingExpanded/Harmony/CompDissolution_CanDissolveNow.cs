using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Verse.AI;
using RimWorld.Planet;



namespace VanillaRecyclingExpanded
{



    [HarmonyPatch(typeof(CompDissolution))]
    [HarmonyPatch("CanDissolveNow", MethodType.Getter)]
    public static class VanillaRecyclingExpanded_CompDissolution_CanDissolveNow_Patch
    {
        [HarmonyPostfix]
        public static void AvoidDissolutionInCrate(CompDissolution __instance,ref bool __result)

        {
            if (__instance.parent.Map!=null && __instance.parent.Position.GetEdifice(__instance.parent.Map)?.def==InternalDefOf.VRecyclingE_WasteCrate) {

                __result = false;


            }

            

        }
    }













}

