using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public abstract class CompProperties_AbilityPyrinthHeatGated : CompProperties_AbilityEffect
    {
        public float heatCost = 50f;
    }

    public abstract class CompAbilityEffect_PyrinthHeatGated : CompAbilityEffect
    {
        private new CompProperties_AbilityPyrinthHeatGated Props =>
            (CompProperties_AbilityPyrinthHeatGated)props;

        protected PyrinthHeatTracker Heat =>
            PyrinthHeatUtility.GetTracker(parent.pawn);

        public float HeatCost => Props.heatCost;
        
        public override string ExtraTooltipPart()
        {
            return "PyrinthBionics_AbilityHeatCostTooltip".Translate(
                Mathf.FloorToInt(HeatCost).Named("COST")
            ).Resolve();
        }
        
        public override bool GizmoDisabled(out string reason)
        {
            var tracker = Heat;

            if (tracker == null)
            {
                reason = "PyrinthBionics_Gizmo_NoHeatTracker".Translate();
                return true;
            }

            if (!tracker.CanConsume(Props.heatCost))
            {
                reason = "PyrinthBionics_AbilityHeatCostNotEnoughHeat".Translate(
                    Mathf.FloorToInt(Props.heatCost).Named("COST"),
                    Mathf.FloorToInt(tracker.Current).Named("ACTUALHEAT"),
                    Mathf.FloorToInt(tracker.Max).Named("MAXHEAT")
                ).Resolve();
                return true;
            }

            reason = null;
            return false;
        }
    }
}