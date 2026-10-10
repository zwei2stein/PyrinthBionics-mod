using RimWorld;
using Verse;

namespace PyrinthBionics
{
    [DefOf]
    public static class PyrinthBionicsDefOf
    {
        public static HediffDef PyrinthBionics_PyrinthHeat;

        public static TraitDef Pyromaniac;

        public static ThoughtDef PyrinthBionics_Seduction;

        public static StatCategoryDef PyrinthBionics_StatCategory;
        public static StatDef PyrinthBionics_HeatCapacity;
        public static StatDef PyrinthBionics_HeatPerSecond;

        static PyrinthBionicsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PyrinthBionicsDefOf));
        }
    }
}