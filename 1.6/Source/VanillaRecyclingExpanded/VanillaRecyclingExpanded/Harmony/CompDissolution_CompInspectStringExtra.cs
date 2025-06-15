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
using System.Text;

// Patches the CompInspectStringExtra method of the CompDissolution class.
// Patch makes the Inspect String of a Wastepack/Biopack read "Protected (won't dissolve)." when inside of the Waste Crate.

// Godspeed, and thanks for the wonderful mod(s)! - Negitive

namespace VanillaRecyclingExpanded
{



    [HarmonyPatch(typeof(CompDissolution))]
    [HarmonyPatch("CompInspectStringExtra", MethodType.Normal)]
    public static class VanillaRecyclingExpanded_CompDissolution_CompInspectStringExtra_Patch
    {
        [HarmonyPostfix]
        public static void InspectStringInformOfProtection(CompDissolution __instance, ref string __result)

        {
            if (__instance.parent.StoringThing() is Building_WasteCrate) {

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append("VRecyclingE_DissolutionWasteCrate".Translate());

                __result = stringBuilder.ToString();


            }



        }
    }












}