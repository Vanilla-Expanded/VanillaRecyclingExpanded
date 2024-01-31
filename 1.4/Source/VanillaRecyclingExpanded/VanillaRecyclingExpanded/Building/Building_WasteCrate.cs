
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

             
             Vector3 drawPos2 = DrawPos;
             drawPos2.y -= 3f / 148f;
             def.building.wastepackAtomizerWindowGraphic.Graphic.Draw(drawPos2, base.Rotation, this);



         }


     }
}