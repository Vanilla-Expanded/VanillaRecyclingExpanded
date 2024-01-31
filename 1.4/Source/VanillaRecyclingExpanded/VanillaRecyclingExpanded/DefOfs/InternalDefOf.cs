
using RimWorld;
using Verse;


namespace VanillaRecyclingExpanded
{
    [DefOf]
    public static class InternalDefOf
    {


        public static SoundDef VRecyclingE_Compactor;
        public static SoundDef VRecyclingE_MeleeHit_TrashBrick;

        public static ThingDef WastepackAtomizer;
        public static ThingDef VRecyclingE_GarbageCompactor;
        public static ThingDef VRecyclingE_AlloypackSplitter;

        public static JobDef VRecyclingE_HaulToCompactor;
        public static JobDef VRecyclingE_HaulToAlloySplitter;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
