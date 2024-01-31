
using RimWorld;
using Verse;
using Verse.Sound;

using Verse.Noise;
using UnityEngine;
using System;

namespace VanillaRecyclingExpanded
{
    public class Building_AlloySplitter : Building
    {
        private CompSuperSimpleProcessor comp;
        private CompPowerTrader powerTrader;
        public int tickCounter =0;
        public const int interval = 600;
        private Effecter operatingEffecter;

        public override void ExposeData()
        {
            base.ExposeData();
   
            Scribe_Values.Look(ref tickCounter, "tickCounter");

        }



        private CompPowerTrader PowerTrader
        {
            get
            {
                if (powerTrader == null)
                {
                    powerTrader = GetComp<CompPowerTrader>();
                }
                return powerTrader;
            }
        }

        public CompSuperSimpleProcessor Comp
        {
            get
            {
                if (comp == null)
                {
                    comp = GetComp<CompSuperSimpleProcessor>();
                }
                return comp;
            }
        }


        public override void Tick()
        {
            base.Tick();

            if (Comp.Empty || !PowerTrader.PowerOn)
            {
                operatingEffecter?.Cleanup();
                operatingEffecter = null;
                return;
            }
            if (operatingEffecter == null)
            {
                operatingEffecter = def.building.wastepackAtomizerOperationEffecter.Spawn();
                operatingEffecter.Trigger(this, new TargetInfo(InteractionCell, base.Map));
            }
            operatingEffecter.EffectTick(this, new TargetInfo(InteractionCell, base.Map));
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

      
    }
}