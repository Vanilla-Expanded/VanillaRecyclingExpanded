
using RimWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
namespace VanillaRecyclingExpanded
{
    public class PlaceWorker_AlloySplitter : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thingToPlace = null)
        {
            List<IntVec3> outputSlots = new List<IntVec3>() { loc + new IntVec3(1,0,-1).RotatedBy(rot),
            loc + new IntVec3(0,0,-1).RotatedBy(rot),loc + new IntVec3(-1,0,-1).RotatedBy(rot)};

            foreach(IntVec3 cell in outputSlots)
            {
                if (cell.InBounds(map))
                {
                    foreach (Thing item in map.thingGrid.ThingsListAtFast(cell))
                    {
                        Building itemBuilding = item as Building;
                        if(itemBuilding!=null && itemBuilding.def.building.isEdifice)
                        {
                            return new AcceptanceReport("VRecyclingE_AlloySplitterOutputBlocked".Translate());

                        }
                    }
                }
                else{
                    return new AcceptanceReport("VRecyclingE_AlloySplitterOutOfBounds".Translate());
                }
                
                
                  

            }


            return true;
        }
    }
}