using RimWorld;

namespace PyrinthBionics
{
    public class Verb_PyrinthJump : Verb_CastAbilityJump
    {
        protected override bool TryCastShot()
        {
            var heatComp = ability?.CompOfType<CompAbilityEffect_PyrinthHeatGated>();
            if (heatComp == null)
                return false;

            var heat = PyrinthHeatUtility.GetTracker(CasterPawn);
            if (heat == null || !heat.TryConsume(heatComp.HeatCost))
                return false;

            return base.TryCastShot();
        }
    }
}