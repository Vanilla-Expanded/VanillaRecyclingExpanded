﻿
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

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
