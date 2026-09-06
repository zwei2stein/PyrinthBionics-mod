using System.Globalization;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class Gizmo_PyrinthHeat : Gizmo
    {
        private readonly PyrinthHeatTracker tracker;

        public Gizmo_PyrinthHeat(PyrinthHeatTracker tracker)
        {
            this.tracker = tracker;
            Order = -100f;
        }

        public override float GetWidth(float maxWidth)
        {
            return Height;
        }

        public override GizmoResult GizmoOnGUI(
            Vector2 topLeft,
            float maxWidth,
            GizmoRenderParms parms)
        {
            var rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), Height);

            Widgets.DrawWindowBackground(rect);

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;

            // Label
            Widgets.Label(
                new Rect(rect.x + 4f, rect.y + 3f, rect.width - 8f, 16f),
                "PyrinthBionics_Gizmo_Gauge_Label".Translate()
            );

            // Gauge
            var barRect = new Rect(
                rect.x + 4f,
                rect.y + 21f,
                rect.width - 8f,
                rect.height - 44f
            );

            Widgets.FillableBar(
                barRect,
                tracker.Percent,
                SolidColorMaterials.NewSolidColorTexture(new Color(0.753f, 0.459f, 0.302f))
            );

            Widgets.Label(
                barRect,
                "PyrinthBionics_Gizmo_Gauge_Heat_Label".Translate(
                    Mathf.FloorToInt(tracker.Current).Named("ACTUALHEAT"),
                    Mathf.FloorToInt(tracker.Max).Named("MAXHEAT")
                )
            );

            Widgets.Label(
                new Rect(rect.x + 4f, rect.y + 53f, rect.width - 8f, 16f),
                "PyrinthBionics_Gizmo_Gauge_HeatPS_Label".Translate(
                    tracker.GetEffectiveHeatPerSecond().ToString("F1", CultureInfo.InvariantCulture).Named("HEATPS")
                )
            );

            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;

            TooltipHandler.TipRegion(rect, GetTooltip());

            return new GizmoResult(GizmoState.Clear);
        }

        private string GetTooltip()
        {
            return "PyrinthBionics_Gizmo_Gauge_Tooltip".Translate(
                Mathf.FloorToInt(tracker.Current).Named("ACTUALHEAT"),
                Mathf.FloorToInt(tracker.Max).Named("MAXHEAT"),
                tracker.ImplantCount.Named("IMPLANTCOUNT"),
                tracker.GetEffectiveHeatPerSecond().ToString("F1", CultureInfo.InvariantCulture).Named("HEATPS")
            ).Resolve();
        }
    }
}