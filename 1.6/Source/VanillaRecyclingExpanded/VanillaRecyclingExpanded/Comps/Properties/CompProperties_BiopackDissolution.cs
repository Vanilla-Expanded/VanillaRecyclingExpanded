
using RimWorld;
using Verse;
namespace VanillaRecyclingExpanded
{
    public class CompProperties_BiopackDissolution : CompProperties
    {
        public int dissolutionAfterDays = 4;

        public float dissolutinFactorIndoors = 0.5f;

        public float dissolutionFactorRain = 2f;

        public CompProperties_BiopackDissolution()
        {
            compClass = typeof(CompBiopackDissolution);
        }
    }
}
