
using RimWorld;
using System;
using UnityEngine;
using UnityEngine.Analytics;
using Verse;
using System.Collections.Generic;
using Verse.Noise;

namespace VanillaRecyclingExpanded
{
    public class CompSuperSimpleProcessor : CompThingContainer
    {



        public new CompProperties_SuperSimpleProcessor Props => (CompProperties_SuperSimpleProcessor)props;


        private bool autoLoad = true;

        private int ticksAtomized;

        private CompPowerTrader powerComp;

        private Effecter workingEffecter;

        public bool AutoLoad => autoLoad;

        public int SpaceLeft => Props.stackLimit - base.TotalStackCount;

        public int TicksPerAtomize => Props.ticksToFinish;

        public int TicksLeftUntilAllAtomized => base.TotalStackCount * TicksPerAtomize - ticksAtomized;

        public float FillPercent => (float)base.TotalStackCount / (float)Props.stackLimit;
        private Graphic contentsGraphic;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!ModLister.CheckBiotech("Atomizer"))
            {
                parent.Destroy();
                return;
            }
            base.PostSpawnSetup(respawningAfterLoad);
            powerComp = parent.TryGetComp<CompPowerTrader>();
        }

        public override bool Accepts(Thing thing)
        {
            if (base.Accepts(thing))
            {
                return thing.def == Props.thingDef;
            }
            return false;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent.Faction == Faction.OfPlayer)
            {
                Command_Toggle command_Toggle = new Command_Toggle();
                command_Toggle.defaultLabel = "CommandAutoLoad".Translate(Props.thingDef.label);
                command_Toggle.defaultDesc = "CommandAutoLoadDesc".Translate(Props.thingDef.label);
                command_Toggle.icon = (autoLoad ? TexCommand.ForbidOff : TexCommand.ForbidOn);
                command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
                command_Toggle.isActive = () => autoLoad;
                command_Toggle.toggleAction = (Action)Delegate.Combine(command_Toggle.toggleAction, (Action)delegate
                {
                    autoLoad = !autoLoad;
                });
                yield return command_Toggle;
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "CommandEjectContents".Translate();
                command_Action.defaultDesc = "CommandEjectContentsDesc".Translate(Props.thingDef.label);
                command_Action.icon = ContentFinder<Texture2D>.Get(Props.ejectIcon, true); 
                command_Action.disabled = base.Empty;
                command_Action.action = (Action)Delegate.Combine(command_Action.action, new Action(EjectContents));
                yield return command_Action;
            }
            if (!DebugSettings.ShowDevGizmos)
            {
                yield break;
            }
            Command_Action command_Action2 = new Command_Action();
            command_Action2.defaultLabel = "DEV: Process";
            command_Action2.disabled = base.Empty;
            command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(DoAtomize));
            yield return command_Action2;
            yield return new Command_Action
            {
                defaultLabel = "DEV: Set next processing",
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
                            ticksAtomized = TicksPerAtomize - ticks;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                },
                disabled = base.Empty
            };
        }

        private void DoAtomize()
        {
            Thing containedThing = base.ContainedThing;
            containedThing.stackCount--;

            if (this.parent.Map != null)
            {
                foreach (SimpleResult result in Props.results)
                {
                    TrySpawnAtCell(result.outputCellOffset, result.thingResult, result.count);
                }
            }

            if (containedThing.stackCount <= 0)
            {
                innerContainer.Remove(containedThing);
                containedThing.Destroy();
               
                

            }
            ticksAtomized = 0;
        }

        private void EjectContents()
        {
            innerContainer.TryDropAll(parent.InteractionCell, parent.Map, ThingPlaceMode.Near);
            ticksAtomized = 0;
        }

        private bool TrySpawnAtCell(IntVec3 outputCellOffset, ThingDef thingResult, int count)
        {
            IntVec3 trueOutputCell;
            if (outputCellOffset != IntVec3.Invalid)
            {
                trueOutputCell = parent.Position + outputCellOffset.RotatedBy(parent.Rotation);
            }
            else trueOutputCell = this.parent.InteractionCell;


            if (trueOutputCell.Walkable(this.parent.Map))
            {
               
                var thing = trueOutputCell.GetFirstThing(this.parent.Map, thingResult);
                if (thing != null)
                {
                
                    if ((thing.stackCount + count) > thing.def.stackLimit)
                        return false;
                   
                    thing.stackCount += count;
                    return true;
                }
                else
                {

                    thing = ThingMaker.MakeThing(thingResult);
                    thing.stackCount = count;
                    if (!GenPlace.TryPlaceThing(thing, trueOutputCell, this.parent.Map, ThingPlaceMode.Near))
                        return false;

                 
                    return true;
                }
            }
            return false;

        }



        private Graphic ContentsGraphic
        {
            get
            {
                if (contentsGraphic == null)
                {
                    ThingDef thingDef = Props.thingDef;
                    contentsGraphic = thingDef.graphic.GetColoredVersion(thingDef.graphic.Shader, thingDef.graphic.color, thingDef.graphic.colorTwo);
                }
                return contentsGraphic;
            }
        }
        public override void PostDraw()
        {
            base.PostDraw();
            if (!autoLoad)
            {
                parent.Map.overlayDrawer.DrawOverlay(parent, OverlayTypes.ForbiddenAtomizer);
            }
            if (!base.Empty)
            {
                Vector3 loc = parent.DrawPos + Props.contentsDrawOffset;
                loc.y -= 3f / 74f;
                Graphic_StackCount graphic_StackCount;
                if ((graphic_StackCount = ContentsGraphic as Graphic_StackCount) != null)
                {
                    graphic_StackCount.SubGraphicForStackCount(Mathf.Min(base.TotalStackCount, Props.thingDef.stackLimit), Props.thingDef).Draw(loc, Rot4.North, parent);
                }
                else
                {
                    ContentsGraphic.Draw(loc, Rot4.North, parent);
                }
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (!base.Empty && powerComp.PowerOn)
            {
                ticksAtomized++;
                if (ticksAtomized >= TicksPerAtomize)
                {
                    DoAtomize();
                }
                if (Props.useEffecter) {
                    if (workingEffecter == null)
                    {
                        workingEffecter = Props.workingEffecter.Spawn(parent, parent.Map);
                    }
                    workingEffecter.EffectTick(parent, parent);
                }
                
            }
            else
            {
                workingEffecter?.Cleanup();
                workingEffecter = null;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref autoLoad, "autoLoad", defaultValue: true);
            Scribe_Values.Look(ref ticksAtomized, "ticksAtomized", 0);
        }

        public override string CompInspectStringExtra()
        {
            TaggedString taggedString = "";
            if (!base.Empty)
            {
                taggedString += (string)("VRecyclingE_ContainedThings".Translate(Props.thingDef.label) + ": ") + base.TotalStackCount + " / " + Props.stackLimit;
                taggedString += "\n" + "FinishesIn".Translate() + ": " + "PeriodDays".Translate(((float)TicksLeftUntilAllAtomized / 60000f).ToString("F1"));
                taggedString += "\n" + "VRecyclingE_DurationUntilNextProcess".Translate() + ": " + (TicksPerAtomize - ticksAtomized).ToStringTicksToPeriod();
                if (!powerComp.PowerOn)
                {
                    taggedString += " (" + "Paused".Translate().ToString().UncapitalizeFirst() + ")";
                }
            }
            return taggedString.Resolve();
        }
    }
}
