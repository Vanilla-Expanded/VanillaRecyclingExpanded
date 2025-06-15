
using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
namespace VanillaRecyclingExpanded
{
    public class CompToxGasOnDestroyed : ThingComp
    {
        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (parent.PositionHeld != IntVec3.Invalid && previousMap!=null)
            {
                GasUtility.AddGas(parent.PositionHeld, previousMap, GasType.RotStink, 10);
               
            }
             base.PostDestroy(mode, previousMap);
        }
    }
}