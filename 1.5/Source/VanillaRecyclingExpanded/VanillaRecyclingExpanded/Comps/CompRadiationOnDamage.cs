
using RimWorld;
using System;
using UnityEngine;
using UnityEngine.Analytics;
using Verse;
using System.Collections.Generic;

namespace VanillaRecyclingExpanded
{
    public class CompRadiationOnDamage : ThingComp
    {

        public bool isLeaking = false;

        private CompProperties_RadiationOnDamage Props => (CompProperties_RadiationOnDamage)props;

        public override void PostExposeData()
        {

            Scribe_Values.Look(ref isLeaking, "isLeaking", defaultValue: false);
            base.PostExposeData();
        }

        private bool IsPawnAffected(Pawn target)
        {
            if (target==null || target.Dead || target.health == null || target.GetStatValue(StatDefOf.ToxicEnvironmentResistance) >=1)
            {
               
                return false;
            }
            if (target.Position.DistanceTo(parent.Position) <= Props.baseRadius*this.parent.stackCount)
            {
               
                return true;
            }
            return false;
        }

        public override void CompTick()
        {
            base.CompTick();
            if (isLeaking && this.parent.IsHashIntervalTick(60) && this.parent.Map != null)
            {
                Dictionary<Pawn,float> listOfAffectedPawns = new Dictionary<Pawn, float>();

                foreach (Pawn pawn in parent.Map.mapPawns.AllPawnsSpawned)
                {
                    if (!IsPawnAffected(pawn))
                    {
                        continue;
                    }
                    float num = 0.01f;
                    num *= Mathf.Max(1f - pawn.GetStatValue(StatDefOf.ToxicResistance), 0f);                 
                    num *= Mathf.Max(1f - pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance), 0f);
                  
                    if (num != 0f)
                    {
                        listOfAffectedPawns.Add(pawn, num);
                        
                    }
                }
                if(listOfAffectedPawns.Count > 0)
                {
                    foreach(Pawn pawn in listOfAffectedPawns.Keys)
                    {
                        HealthUtility.AdjustSeverity(pawn, HediffDefOf.ToxicBuildup, listOfAffectedPawns[pawn]);
                    }
                }
            }
        }

        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(ref dinfo, out absorbed);          
            isLeaking = true;          
        }
        public override string CompInspectStringExtra()
        {
            if (isLeaking)
            {
                return base.CompInspectStringExtra() + "VRecyclingE_Leaking".Translate();
            }
            else
            return base.CompInspectStringExtra();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();
            if (isLeaking && this.parent.Map!=null)
            {
                GenDraw.DrawRadiusRing(this.parent.PositionHeld, Props.baseRadius * this.parent.stackCount, Color.green);
            }
        }
       
    }
}
