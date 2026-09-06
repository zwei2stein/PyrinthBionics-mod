using RimWorld;
using Verse;

namespace PyrinthBionics
{
    [DefOf]
    public static class PyrinthBionicsDefOf
    {
        public static HediffDef Heatstroke;
        public static HediffDef Hypothermia;

        public static HediffDef PyrinthBionics_PyrinthHeat;

        public static TraitDef Pyromaniac;

        public static ThoughtDef PyrinthBionics_Seduction;

        public static StatCategoryDef PyrinthBionics_StatCategory;

        static PyrinthBionicsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PyrinthBionicsDefOf));
        }
    }
}