
using RimWorld;
using Verse;
namespace VanillaRecyclingExpanded
{
    public class CompProperties_RadiationOnDamage : CompProperties
    {
       

        public float baseRadius = 3f;
      

        public CompProperties_RadiationOnDamage()
        {
            compClass = typeof(CompRadiationOnDamage);
        }
    }
}
