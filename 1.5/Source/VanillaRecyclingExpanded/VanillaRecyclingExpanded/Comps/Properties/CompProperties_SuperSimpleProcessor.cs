
using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;


namespace VanillaRecyclingExpanded
{
    public class CompProperties_SuperSimpleProcessor : CompProperties_ThingContainer
    {
        public ThingDef thingDef;

        public int ticksToFinish = 2500;

        public Vector3 contentsDrawOffset = Vector3.zero;

        public bool useEffecter = true;

        public EffecterDef workingEffecter;

        public List<SimpleResult> results;

        public string ejectIcon = "";

        public CompProperties_SuperSimpleProcessor()
        {
            compClass = typeof(CompSuperSimpleProcessor);
        }

    }

    public class SimpleResult
    {
        public ThingDef thingResult;      
        public int count;  
        public IntVec3 outputCellOffset = IntVec3.Invalid; 
    }
}
