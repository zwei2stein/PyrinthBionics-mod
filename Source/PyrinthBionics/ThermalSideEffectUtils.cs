using System;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public static class ThermalSideEffectUtils
    {
        public static void ApplyThermalSideEffect(Pawn targetPawn, FloatRange heatStrokeSideEffect)
        {
            var heatStrokeSideEffectActual = heatStrokeSideEffect.RandomInRange;
                
            var hypothermia = targetPawn.health.hediffSet.GetFirstHediffOfDef(PyrinthBionicsDefOf.Hypothermia);

            if (hypothermia != null && hypothermia.Severity > 0f)
            {
                var hypothermiaSeverityReductionLeftover =
                    Math.Max(0, heatStrokeSideEffectActual - hypothermia.Severity);
                hypothermia.Severity = Mathf.Max(0f, hypothermia.Severity - heatStrokeSideEffectActual);

                if (hypothermiaSeverityReductionLeftover > 0)
                {
                    var heatstrokeFromLeftover = HediffMaker.MakeHediff(PyrinthBionicsDefOf.Heatstroke, targetPawn);
                    heatstrokeFromLeftover.Severity = hypothermiaSeverityReductionLeftover;
                    targetPawn.health.AddHediff(heatstrokeFromLeftover);
                }

                return;
            }

            var heatstroke = targetPawn.health.hediffSet.GetFirstHediffOfDef(PyrinthBionicsDefOf.Heatstroke);
            if (heatstroke != null)
            {
                heatstroke.Severity += heatStrokeSideEffectActual;
            }
            else
            {
                heatstroke = HediffMaker.MakeHediff(PyrinthBionicsDefOf.Heatstroke, targetPawn);
                heatstroke.Severity = heatStrokeSideEffectActual;
                targetPawn.health.AddHediff(heatstroke);
            }
        }
    }
}