
using RimWorld;
using Verse;
using Verse.Sound;
using Verse.Noise;
using UnityEngine;
using System;

namespace VanillaRecyclingExpanded
{
    public class Building_Compactor : Building
    {
        private CompSuperSimpleProcessor comp;
        public int tickCounter =0;
        public const int interval = 600;
        public Graphic_Multi graphic = null;


        public override void ExposeData()
        {
            base.ExposeData();
   
            Scribe_Values.Look(ref tickCounter, "tickCounter");

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            comp = this.TryGetComp<CompSuperSimpleProcessor>();
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
 
                graphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/GarbageCompactor/GarbageCompactorLid", ShaderDatabase.DefaultShader, DrawSize, DrawColor);
            }
            catch (Exception) { }
        }



        public override void Tick()
        {
            base.Tick();
            
            if (comp!=null&&!comp.Empty)
            {
                tickCounter++;
                if (this.IsHashIntervalTick(interval))
                {
                    InternalDefOf.VRecyclingE_Compactor.PlayOneShot(new TargetInfo(this.Position, this.Map));
                    GasUtility.AddGas(this.InteractionCell, this.MapHeld, GasType.ToxGas, 300);
                    tickCounter = 0;
                }
            }
            
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {

            base.DrawAt(drawLoc, flip);
            var vector = DrawPos;
            vector.y += 6;
            if (comp != null && !comp.Empty)
            {
               
                float height = Mathf.Lerp(0, 0.5f, (float)tickCounter / 600);
            
                vector.z += height;
              

            }

            GetGraphic?.DrawFromDef(vector, this.Rotation, null);
          
        }

        public override void Notify_ColorChanged()
        {
            base.Notify_ColorChanged();
            graphic = null;
            Map.mapDrawer.MapMeshDirty(Position, MapMeshFlagDefOf.Things);
            DrawAt(this.DrawPos);
        }
    }
}