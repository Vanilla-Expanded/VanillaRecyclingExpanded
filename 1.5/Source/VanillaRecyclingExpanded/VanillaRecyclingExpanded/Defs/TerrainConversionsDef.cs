using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace VanillaRecyclingExpanded
{
    public class TerrainConversionsDef : Def
    {
       
        public List<TerrainEquivalence> terrainConversions;
    }

    public class TerrainEquivalence
    {
        public string terrainToConvert;

        public string terrainToConvertTo;

    }


}