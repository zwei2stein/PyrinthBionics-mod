using Verse;

namespace PyrinthBionics
{
    public class HediffCompProperties_PyrinthHeatBoost : HediffCompProperties
    {
        public float heatPerSecondBonus = 1f;

        public HediffCompProperties_PyrinthHeatBoost()
        {
            compClass = typeof(HediffComp_PyrinthHeatBoost);
        }
    }

    public class HediffComp_PyrinthHeatBoost : HediffComp
    {
        public HediffCompProperties_PyrinthHeatBoost Props =>
            (HediffCompProperties_PyrinthHeatBoost)props;

        public float HeatPerSecondBonus => Props.heatPerSecondBonus;
        
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            PyrinthHeatUtility.GetTracker(parent.pawn)?.Recalculate();
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            PyrinthHeatUtility.GetTracker(parent.pawn)?.Recalculate();
        }
    }
}