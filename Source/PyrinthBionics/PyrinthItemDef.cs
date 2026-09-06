using System.Collections.Generic;
using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class PyrinthItemDef : ThingDef
    {
        public HediffDef linkedHediff;

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (var entry in base.SpecialDisplayStats(req))
                yield return entry;

            var ext = linkedHediff?.GetModExtension<PyrinthBionicsImplantExtension>();

            foreach (var entry in PyrinthBionicsUtility.HeatStatEntries(ext))
                yield return entry;
        }
    }

    public class PyrinthPotionItemDef : ThingDef
    {
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (var entry in base.SpecialDisplayStats(req))
                yield return entry;

            if (ingestible?.outcomeDoers == null)
                yield break;

            foreach (var outcomeDoer in ingestible.outcomeDoers)
            {
                // Refill effect
                if (outcomeDoer is IngestionOutcomeDoer_FillPyrinthHeat)
                {
                    yield return new StatDrawEntry(
                        PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                        "PyrinthBionics_StatEntry_RefillMaxLabel".Translate(),
                        "PyrinthBionics_StatEntry_RefillMaxValue".Translate(),
                        "PyrinthBionics_StatEntry_RefillMaxDesc".Translate(),
                        1000);
                }

                // Hediff effect: heat boost & hypothermia treatment
                if (outcomeDoer is IngestionOutcomeDoer_GiveHediff giveHediff && giveHediff.hediffDef != null)
                {
                    var hediffDef = giveHediff.hediffDef;

                    var heatProps = hediffDef.CompProps<HediffCompProperties_PyrinthHeatBoost>();
                    if (heatProps != null)
                    {
                        yield return new StatDrawEntry(
                            PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                            "PyrinthBionics_StatEntry_HeatPerSecondLabel".Translate(),
                            "PyrinthBionics_Gizmo_Gauge_HeatPS_Label".Translate(heatProps.heatPerSecondBonus.ToString("F1").Named("HEATPS")),
                            "PyrinthBionics_StatEntry_HeatPerSecondDesc".Translate(),
                            999);
                    }

                    var hypoProps = hediffDef.CompProps<HediffCompProperties_PyrinthHypothermiaTreatment>();
                    if (hypoProps != null)
                    {
                        yield return new StatDrawEntry(
                            PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                            "PyrinthBionics_StatEntry_HypothermiaReductionLabel".Translate(),
                            "PyrinthBionics_StatEntry_HypothermiaReductionValue".Translate(
                                (hypoProps.severityReductionPerSecond).ToString("F2").Named("REDUCTION")),
                            "PyrinthBionics_StatEntry_HypothermiaReductionDesc".Translate(),
                            998);
                    }
                }
            }
        }
    }
    
}