using Verse;

namespace PyrinthBionics
{
    public static class PyrinthHeatUtility
    {
        public static Hediff_PyrinthHeat GetHeatHediff(Pawn pawn)
        {
            return pawn?.health?.hediffSet?
                .GetFirstHediffOfDef(PyrinthBionicsDefOf.PyrinthBionics_PyrinthHeat) as Hediff_PyrinthHeat;
        }

        public static PyrinthHeatTracker GetTracker(Pawn pawn)
        {
            return GetHeatHediff(pawn)?.Tracker;
        }

        public static void EnsureHeatTracker(Pawn pawn)
        {
            if (pawn == null)
                return;

            if (GetHeatHediff(pawn) != null)
                return;

            var heat = HediffMaker.MakeHediff(PyrinthBionicsDefOf.PyrinthBionics_PyrinthHeat, pawn);
            pawn.health.AddHediff(heat);
        }

        public static void RemoveHeatTrackerIfUnused(Pawn pawn)
        {
            if (pawn == null)
                return;

            if (PyrinthBionicsUtility.CountImplants(pawn) > 0)
                return;

            Hediff heat = GetHeatHediff(pawn);

            if (heat != null)
                pawn.health.RemoveHediff(heat);
        }
    }
}