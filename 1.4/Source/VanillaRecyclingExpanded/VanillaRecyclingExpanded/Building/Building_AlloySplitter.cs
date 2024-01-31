
using RimWorld;
using Verse;
using Verse.Sound;

using Verse.Noise;
using UnityEngine;
using System;

namespace VanillaRecyclingExpanded
{
   /* public class Building_AlloySplitter : Building
    {
        private CompAdvancedResourceProcessor comp;
        public int tickCounter =0;
        public const int interval = 600;
        private Effecter operatingEffecter;

        public override void ExposeData()
        {
            base.ExposeData();
   
            Scribe_Values.Look(ref tickCounter, "tickCounter");

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            comp = this.TryGetComp<CompAdvancedResourceProcessor>();
        }





        public override void Tick()
        {
            base.Tick();

            if (comp?.Process?.IsRunning == false)
            {
                operatingEffecter?.Cleanup();
                operatingEffecter = null;
                return;
            }
            if (operatingEffecter == null)
            {
                operatingEffecter = InternalDefOf.WastepackAtomizer.building.wastepackAtomizerOperationEffecter.Spawn();
                operatingEffecter.Trigger(this, new TargetInfo(PositionHeld, base.Map));
            }

           
          
            operatingEffecter.EffectTick(this, new TargetInfo(this.PositionHeld, base.Map));
        }

        public override void Draw()
        {
           
            base.Draw();

            Vector3 drawPos = DrawPos;
            drawPos.y -= 0.08108108f;
            def.building.wastepackAtomizerBottomGraphic.Graphic.Draw(drawPos, base.Rotation, this);
            Vector3 drawPos2 = DrawPos;
            drawPos2.y -= 3f / 148f;
            def.building.wastepackAtomizerWindowGraphic.Graphic.Draw(drawPos2, base.Rotation, this);

   

        }

      
    }*/
}