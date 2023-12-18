
using RimWorld;
using Verse;


namespace VanillaRecyclingExpanded
{
    [DefOf]
    public static class InternalDefOf
    {


        public static SoundDef VRecyclingE_Compactor;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
