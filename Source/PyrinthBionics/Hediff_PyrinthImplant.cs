using System.Collections.Generic;
using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class Hediff_PyrinthImplant : Hediff_AddedPart
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            PyrinthHeatUtility.EnsureHeatTracker(pawn);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            PyrinthHeatUtility.RemoveHeatTrackerIfUnused(pawn);
        }
    }

    public class Hediff_PyrinthPotionEffect : HediffWithComps
    {

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (var entry in base.SpecialDisplayStats(req))
                yield return entry;
            
            // Hypothermia reduction
            var hypoProps = def.CompProps<HediffCompProperties_PyrinthHypothermiaTreatment>();
            if (hypoProps != null)
            {
                yield return new StatDrawEntry(
                    PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                    "PyrinthBionics_StatEntry_HypothermiaReductionLabel".Translate(),
                    "PyrinthBionics_StatEntry_HypothermiaReductionValue".Translate(
                        hypoProps.severityReductionPerSecond.ToString("F2").Named("REDUCTION")),
                    "PyrinthBionics_StatEntry_HypothermiaReductionDesc".Translate(),
                    999);
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            PyrinthHeatUtility.EnsureHeatTracker(pawn);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            PyrinthHeatUtility.RemoveHeatTrackerIfUnused(pawn);
        }
    }
}