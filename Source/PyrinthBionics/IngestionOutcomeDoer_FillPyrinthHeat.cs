using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class IngestionOutcomeDoer_FillPyrinthHeat : IngestionOutcomeDoer
    {
        public float heatstrokeSeverityGain = 0.02f;
        
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            PyrinthHeatTracker tracker = PyrinthHeatUtility.GetTracker(pawn);

            if (tracker == null)
                return;

            tracker.Current = tracker.Max;
            
            Hediff heatstroke =
                pawn.health?.hediffSet
                    ?.GetFirstHediffOfDef(HediffDefOf.Heatstroke);

            if (heatstroke != null)
            {
                heatstroke.Severity += heatstrokeSeverityGain;
            }
            else
            {
                heatstroke = HediffMaker.MakeHediff(
                    HediffDefOf.Heatstroke,
                    pawn
                );

                heatstroke.Severity = heatstrokeSeverityGain;
                pawn.health.AddHediff(heatstroke);
            }
            
        }
    }
}