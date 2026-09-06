using System.Collections.Generic;
using System.Globalization;
using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class Hediff_PyrinthImplant : Hediff_AddedPart
    {
        public override string TipStringExtra
        {
            get
            {
                var baseText = base.TipStringExtra;
                var ext = def.GetModExtension<PyrinthBionicsImplantExtension>();
                if (ext == null) return baseText;

                return baseText +
                       "PyrinthBionics_Hediff_Tooltip".Translate(
                           Mathf.FloorToInt(ext.heatCapacity).Named("HEATCAPACITY"),
                           ext.heatPerSecond.ToString("F1", CultureInfo.InvariantCulture).Named("HEATPS")
                       ).Resolve();
            }
        }
        
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (var entry in base.SpecialDisplayStats(req))
                yield return entry;

            var ext = def.GetModExtension<PyrinthBionicsImplantExtension>();
            
            foreach (var entry in PyrinthBionicsUtility.HeatStatEntries(ext))
                yield return entry;
        }
        
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            PyrinthHeatUtility.EnsureHeatTracker(pawn);
            PyrinthHeatUtility.GetTracker(pawn)?.Recalculate();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();

            PyrinthHeatUtility.RemoveHeatTrackerIfUnused(pawn);
            PyrinthHeatUtility.GetTracker(pawn)?.Recalculate();
        }
    }
    
    public class Hediff_PyrinthPotionEffect : HediffWithComps
    {
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (var entry in base.SpecialDisplayStats(req))
                yield return entry;

            var heatProps = def.CompProps<HediffCompProperties_PyrinthHeatBoost>();
            if (heatProps != null)
            {
                yield return new StatDrawEntry(
                    PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                    "PyrinthBionics_StatEntry_HeatPerSecondLabel".Translate(),
                    "PyrinthBionics_Gizmo_Gauge_HeatPS_Label".Translate(heatProps.heatPerSecondBonus.ToString("F1").Named("HEATPS")),
                    "PyrinthBionics_StatEntry_HeatPerSecondDesc".Translate(),
                    1000);
            }

            // Hypothermia severity reduction
            var hypoProps = def.CompProps<HediffCompProperties_PyrinthHypothermiaTreatment>();
            if (hypoProps != null)
            {
                yield return new StatDrawEntry(
                    PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                    "PyrinthBionics_StatEntry_HypothermiaReductionLabel".Translate(),
                    "PyrinthBionics_StatEntry_HypothermiaReductionValue".Translate(
                        (hypoProps.severityReductionPerSecond).ToString("F2").Named("REDUCTION")),
                    "PyrinthBionics_StatEntry_HypothermiaReductionDesc".Translate(),
                    999);
            }
        }

        public override string TipStringExtra
        {
            get
            {
                var baseText = base.TipStringExtra;
                var heatProps = def.CompProps<HediffCompProperties_PyrinthHeatBoost>();
                if  (heatProps == null) return baseText;
                
                return baseText +
                       "PyrinthBionics_Hediff_Tooltip".Translate(
                           0.Named("HEATCAPACITY"),
                           heatProps.heatPerSecondBonus.ToString("F1", CultureInfo.InvariantCulture).Named("HEATPS")
                       ).Resolve();
            }
        }
    }
    
}