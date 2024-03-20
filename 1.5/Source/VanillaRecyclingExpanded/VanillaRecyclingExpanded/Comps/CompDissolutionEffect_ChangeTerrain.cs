
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

            if (amount >= 5)
            {
                List<TerrainEquivalence> terrainequivalences = new List<TerrainEquivalence>();
                List<TerrainConversionsDef> allLists = DefDatabase<TerrainConversionsDef>.AllDefsListForReading;
                foreach (TerrainConversionsDef individualList in allLists)
                {
                    terrainequivalences.AddRange(individualList.terrainConversions);
                }

                TerrainDef terrain = this.parent.Position.GetTerrain(this.parent.Map);

                if (terrainequivalences.Count > 0)
                {
                    foreach (TerrainEquivalence terrainequivalence in terrainequivalences)
                    {
                        if (terrainequivalence.terrainToConvert==terrain.defName)
                        {
                            this.parent.Map.terrainGrid.SetTerrain(this.parent.Position, TerrainDef.Named(terrainequivalence.terrainToConvertTo));

                        }
                    }
                }
            }
            float num = parent.HitPoints * 4 * amount;
            
            GasUtility.AddGas(parent.PositionHeld, parent.MapHeld, GasType.RotStink, (int)num);
        }

        public override void DoDissolutionEffectWorld(int amount, int tileId)
        {

        }

    }
}