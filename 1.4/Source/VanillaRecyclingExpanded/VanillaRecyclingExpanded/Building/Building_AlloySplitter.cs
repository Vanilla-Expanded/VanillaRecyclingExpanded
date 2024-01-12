
using RimWorld;
using Verse;
using Verse.Sound;
using PipeSystem;
using Verse.Noise;
using UnityEngine;
using System;

namespace VanillaRecyclingExpanded
{
    public class Building_AlloySplitter : Building
    {
        private CompAdvancedResourceProcessor comp;
        public int tickCounter =0;
        public const int interval = 600;
        public Graphic_Multi graphic = null;
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

        public Graphic_Multi GetGraphic
        {
            get
            {
                if (graphic is null)
                {
                    LongEventHandler.ExecuteWhenFinished(delegate { GetGraphicLong(); });

                }
                return graphic;
            }
        }

        public void GetGraphicLong()
        {
            try
            {
 
                graphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/AlloypackSplitter/AlloypackSplitter_BottomLayer", ShaderDatabase.DefaultShader, DrawSize, DrawColor);
            }
            catch (Exception) { }
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
            drawPos.y -=0.001f;
            GetGraphic?.DrawFromDef(drawPos, this.Rotation, null); 

        }

      
    }
}