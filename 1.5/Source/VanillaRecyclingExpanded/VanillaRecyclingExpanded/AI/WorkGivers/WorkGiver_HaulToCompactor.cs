using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaRecyclingExpanded
{
    public class WorkGiver_HaulToCompactor : WorkGiver_Scanner
    {
        private const float MaxFillPercent = 0.5f;

        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(InternalDefOf.VRecyclingE_GarbageCompactor);

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!ModLister.CheckBiotech("Haul to atomizer"))
            {
                return false;
            }
            if (t.IsForbidden(pawn))
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            CompSuperSimpleProcessor comp = t.TryGetComp<CompSuperSimpleProcessor>();
            if (comp.Full)
            {
                JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans);
                return false;
            }
            if (!forced && !comp.AutoLoad)
            {
                return false;
            }
            if (!forced && comp.FillPercent > MaxFillPercent)
            {
                return false;
            }
            if (HaulAIUtility.FindFixedIngredientCount(pawn, comp.Props.thingDef, comp.SpaceLeft).NullOrEmpty())
            {
                JobFailReason.Is("NoIngredient".Translate(comp.Props.thingDef));
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            CompSuperSimpleProcessor comp = t.TryGetComp<CompSuperSimpleProcessor>();
            if (comp == null)
            {
                return null;
            }
            if (comp.Full)
            {
                JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans);
                return null;
            }
            if (!forced && !comp.AutoLoad)
            {
                return null;
            }
            List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, comp.Props.thingDef, comp.SpaceLeft);
            if (list.NullOrEmpty())
            {
                JobFailReason.Is("NoIngredient".Translate(comp.Props.thingDef));
                return null;
            }
            Job job = JobMaker.MakeJob(InternalDefOf.VRecyclingE_HaulToCompactor, t);
            job.targetQueueB = list.Select((Thing f) => new LocalTargetInfo(f)).ToList();
            job.count = comp.SpaceLeft;
            return job;
        }
    }
}