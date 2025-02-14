
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
        public int tickCounter =0;
        public const int interval = 600;
        private Effecter operatingEffecter;

        public override void ExposeData()
        {
            base.ExposeData();
   
            Scribe_Values.Look(ref tickCounter, "tickCounter");

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

            if (Comp?.Working != true)
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

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
           
            base.DrawAt(drawLoc, flip);

            Vector3 drawPos = DrawPos;
            drawPos.y -= 0.08108108f;
            def.building.wastepackAtomizerBottomGraphic.Graphic.Draw(drawPos, base.Rotation, this);
            Vector3 drawPos2 = DrawPos;
            drawPos2.y -= 3f / 148f;
            def.building.wastepackAtomizerWindowGraphic.Graphic.Draw(drawPos2, base.Rotation, this);

   

        }

      
    }
}