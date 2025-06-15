
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace VanillaRecyclingExpanded
{
    public class CompDissolutionEffect_ChangeTerrain : CompDissolutionEffect
    {


        public CompProperties_DissolutionEffectChangeTerrain Props => (CompProperties_DissolutionEffectChangeTerrain)props;

        public override void DoDissolutionEffectMap(int amount)
        {
            Map mapHeld = parent.MapHeld;
            IntVec3 positionHeld = parent.PositionHeld;

            if (amount >= 5)
            {
                List<TerrainEquivalence> terrainequivalences = new List<TerrainEquivalence>();
                List<TerrainConversionsDef> allLists = DefDatabase<TerrainConversionsDef>.AllDefsListForReading;
                foreach (TerrainConversionsDef individualList in allLists)
                {
                    terrainequivalences.AddRange(individualList.terrainConversions);
                }

                TerrainDef terrain = positionHeld.GetTerrain(mapHeld);

                if (terrainequivalences.Count > 0)
                {
                    foreach (TerrainEquivalence terrainequivalence in terrainequivalences)
                    {
                        if (terrainequivalence.terrainToConvert==terrain.defName)
                        {
                            mapHeld.terrainGrid.SetTerrain(positionHeld, TerrainDef.Named(terrainequivalence.terrainToConvertTo));

                        }
                    }
                }
            }
            float num = parent.HitPoints * 4 * amount;
            
            GasUtility.AddGas(positionHeld, mapHeld, GasType.RotStink, (int)num);
        }

     

    }
}