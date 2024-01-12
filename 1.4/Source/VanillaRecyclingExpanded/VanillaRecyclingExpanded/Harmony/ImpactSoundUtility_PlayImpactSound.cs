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



    [HarmonyPatch(typeof(ImpactSoundUtility))]
    [HarmonyPatch("PlayImpactSound")]
    public static class VanillaRecyclingExpanded_ImpactSoundUtility_PlayImpactSound_Patch
    {
        [HarmonyPostfix]
        public static void MakeWallsFartInAVeryVeryConvolutedWay(Thing hitThing)

        {
            if(hitThing?.Stuff?.stuffProps?.soundImpactStuff == InternalDefOf.VRecyclingE_MeleeHit_TrashBrick && hitThing.Map!=null)
            {
                IntVec3 cell = IntVec3.Zero;
                if (hitThing.def.passability != Traversability.Impassable)
                {
                    cell = CellFinder.RandomClosewalkCellNear(hitThing.PositionHeld, hitThing.Map, 2);                   
                }
                else
                {
                    cell = hitThing.OccupiedRect().ExpandedBy(1).RandomCell;

                }
                if (cell != IntVec3.Zero)
                {
                    GasUtility.AddGas(cell, hitThing.Map, GasType.ToxGas, 150);
                }


            }

        }
    }













}

