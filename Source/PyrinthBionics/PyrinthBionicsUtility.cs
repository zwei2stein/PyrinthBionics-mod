using Verse;

namespace PyrinthBionics
{

    public static class PyrinthBionicsUtility
    {

        public static int CountImplants(Pawn pawn)
        {
            var hediffs = pawn?.health?.hediffSet?.hediffs;
            if (hediffs == null)
                return 0;

            var count = 0;
            for (var i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i] is Hediff_PyrinthImplant)
                    count++;
            }

            return count;
        }

    }
}