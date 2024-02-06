
using RimWorld;
using Verse;
using Verse.Sound;

using Verse.Noise;
using UnityEngine;
using System;

namespace VanillaRecyclingExpanded
{
     public class Building_WasteCrate : Building_Storage
    {
       

        





       
         public override void Draw()
         {

             base.Draw();

            Vector3 drawPos = DrawPos;
            drawPos.y += 5;
            def.building.wastepackAtomizerBottomGraphic.Graphic.Draw(drawPos, base.Rotation, this);
            Vector3 drawPos2 = DrawPos;
            drawPos2.y += 6;
            def.building.wastepackAtomizerWindowGraphic.Graphic.DrawFromDef(drawPos2, base.Rotation,null);



         }


     }
}