﻿
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using RimWorld.Planet;

namespace VanillaRecyclingExpanded
{
    public class CompBiopackDissolution : ThingComp
    {
        private int dissolveTicks;

        private static List<string> tmpDissolutionReasons = new List<string>();

        private CompProperties_BiopackDissolution Props => (CompProperties_BiopackDissolution)props;

        public int DeterioarationRate => (int)parent.GetStatValue(StatDefOf.DeteriorationRate);

        public int DissolutionAfterDamage => DeterioarationRate * Props.dissolutionAfterDays;

        public int DefaultTicksUntilDissolution => 60000 * Props.dissolutionAfterDays;

        public bool IsOutdoors
        {
            get
            {
                Map mapHeld = parent.MapHeld;
                IntVec3 position = parent.Position;
                if (mapHeld != null)
                {
                    Room room = position.GetRoom(mapHeld);
                    if (room != null && room.UsesOutdoorTemperature)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsFrozen => parent.AmbientTemperature <= 0f;

        public bool InAtomizer
        {
            get
            {
                if (parent.ParentHolder != null)
                {
                    return parent.ParentHolder is CompAtomizer;
                }
                return false;
            }
        }

        public bool CanDissolveNow
        {
            get
            {
                if (parent.Map != null && parent.Position.GetEdifice(parent.Map)?.def == InternalDefOf.VRecyclingE_WasteCrate)
                {
                    return false;
                }       
                if (!IsFrozen)
                {
                    return !InAtomizer;
                }
                return false;
            }
        }

        public bool IsBeingRainedOn
        {
            get
            {
                if (IsOutdoors)
                {
                    return parent.MapHeld.weatherManager.curWeather.rainRate > 0.1f;
                }
                return false;
            }
        }

        public int DissolutionIntervalTicks
        {
            get
            {
                float num = DefaultTicksUntilDissolution;
                if (!IsOutdoors)
                {
                    num /= Props.dissolutinFactorIndoors;
                }
                if (IsBeingRainedOn)
                {
                    num /= Props.dissolutionFactorRain;
                }
                return (int)num;
            }
        }

        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            base.PreAbsorbStack(otherStack, count);
            dissolveTicks = 0;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!ModLister.CheckBiotech("Dissolution"))
            {
                parent.Destroy();
            }
            else
            {
                base.PostSpawnSetup(respawningAfterLoad);
            }
        }

        public void TriggerDissolutionEvent(int amountToDissolve = 1)
        {
            amountToDissolve =  parent.stackCount;
            if (parent.MapHeld != null)
            {
                DissolveMap(amountToDissolve);
            }
            else
            {
                DissolveWorld(amountToDissolve, parent.Tile);
            }
            parent.stackCount -= amountToDissolve;
            dissolveTicks = 0;
            if (parent.stackCount <= 0 && !parent.Destroyed)
            {
                parent.Destroy();
            }
        }

        private void DissolveMap(int amount)
        {
            List<ThingComp> allComps = parent.AllComps;
            for (int i = 0; i < allComps.Count; i++)
            {
                CompDissolutionEffect compDissolutionEffect;
                if ((compDissolutionEffect = allComps[i] as CompDissolutionEffect) != null)
                {
                    compDissolutionEffect.DoDissolutionEffectMap(amount);
                }
            }
            Find.Storyteller.Notify_DissolutionEvent(parent);
        }

        private void DissolveWorld(int amount, int tile)
        {
            List<ThingComp> allComps = parent.AllComps;
            for (int i = 0; i < allComps.Count; i++)
            {
                CompDissolutionEffect compDissolutionEffect;
                if ((compDissolutionEffect = allComps[i] as CompDissolutionEffect) != null)
                {
                    compDissolutionEffect.DoDissolutionEffectWorld(amount, tile);
                }
            }
        }

        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(ref dinfo, out absorbed);
            if (dinfo.Def.harmsHealth && dinfo.Amount >= (float)parent.HitPoints)
            {
                DissolveMap(parent.stackCount);
            }
        }

        public override void Notify_AbandonedAtTile(PlanetTile tile)
        {
            DissolveWorld(parent.stackCount, tile);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (CanDissolveNow)
            {
                dissolveTicks++;
                if (dissolveTicks >= DissolutionIntervalTicks)
                {
                    TriggerDissolutionEvent();
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (!DebugSettings.ShowDevGizmos)
            {
                yield break;
            }
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "DEV: Dissolution event";
            command_Action.action = delegate
            {
                TriggerDissolutionEvent();
            };
            yield return command_Action;
            Command_Action command_Action2 = new Command_Action();
            command_Action2.defaultLabel = "DEV: Dissolution event until destroyed";
            command_Action2.action = delegate
            {
                int num = 1000;
                while (!parent.Destroyed && num > 0)
                {
                    TriggerDissolutionEvent();
                    num--;
                }
            };
            yield return command_Action2;
            Command_Action command_Action3 = new Command_Action();
            command_Action3.defaultLabel = "DEV: Dissolution progress +25%";
            command_Action3.action = delegate
            {
                parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)DissolutionAfterDamage * 0.25f));
            };
            yield return command_Action3;
            yield return new Command_Action
            {
                defaultLabel = "DEV: Set next dissolve time",
                action = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    int[] array = new int[11]
                    {
                    60, 120, 180, 240, 300, 600, 900, 1200, 1500, 1800,
                    3600
                    };
                    foreach (int ticks in array)
                    {
                        list.Add(new FloatMenuOption(ticks.ToStringSecondsFromTicks("F0"), delegate
                        {
                            dissolveTicks = DissolutionIntervalTicks - ticks;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                }
            };
        }

        public override string CompInspectStringExtra()
        {
            if (InAtomizer)
            {
                return base.CompInspectStringExtra();
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }
            if (IsFrozen)
            {
                stringBuilder.Append("DissolutionFrozen".Translate());
                return stringBuilder.ToString();
            }
            tmpDissolutionReasons.Clear();
            if (!IsOutdoors)
            {
                tmpDissolutionReasons.Add("DissolutionRateIndoors".Translate() + " x" + Props.dissolutinFactorIndoors.ToStringPercent());
            }
            if (IsBeingRainedOn)
            {
                tmpDissolutionReasons.Add("DissolutionRain".Translate() + " x" + Props.dissolutionFactorRain.ToStringPercent());
            }
            stringBuilder.Append("DissolvesEvery".Translate(DissolutionIntervalTicks.ToStringTicksToPeriod()));
            if (tmpDissolutionReasons.Count > 0)
            {
                string str = tmpDissolutionReasons.ToLineList();
                stringBuilder.Append(" (" + str.CapitalizeFirst() + ")");
                tmpDissolutionReasons.Clear();
            }
            return stringBuilder.ToString();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref dissolveTicks, "dissolveTicks", 0);
        }
    }
}