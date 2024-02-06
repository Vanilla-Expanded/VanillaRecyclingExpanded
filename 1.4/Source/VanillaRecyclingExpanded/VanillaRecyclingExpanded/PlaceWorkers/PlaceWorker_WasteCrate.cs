
using RimWorld;
using UnityEngine;
using Verse;
namespace VanillaRecyclingExpanded
{
    public class PlaceWorker_WasteCrate : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Single>(def.building.wastepackAtomizerBottomGraphic.texPath, ShaderDatabase.Cutout, def.building.wastepackAtomizerBottomGraphic.drawSize, Color.white), def, ghostCol).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def);
        }
    }
}