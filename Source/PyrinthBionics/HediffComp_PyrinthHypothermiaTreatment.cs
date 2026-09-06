using Verse;

namespace PyrinthBionics
{
    public class HediffCompProperties_PyrinthHypothermiaTreatment : HediffCompProperties
    {
        public float severityReductionPerSecond = 0.01f;

        public HediffCompProperties_PyrinthHypothermiaTreatment()
        {
            compClass = typeof(HediffComp_PyrinthHypothermiaTreatment);
        }
    }

    public class HediffComp_PyrinthHypothermiaTreatment : HediffComp
    {
        public HediffCompProperties_PyrinthHypothermiaTreatment Props => (HediffCompProperties_PyrinthHypothermiaTreatment) props;

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);
            
            if (parent.pawn.health?.hediffSet == null)
                return;
            
            var tickSeverity = Props.severityReductionPerSecond * delta / GenTicks.TicksPerRealSecond;
            
            ThermalSideEffectUtils.ApplyThermalSideEffect(parent.pawn, new FloatRange(tickSeverity));

        }
    }
}