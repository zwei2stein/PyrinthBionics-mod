namespace PyrinthBionics
{
    public class CompProperties_AbilityImplantJump : CompProperties_AbilityPyrinthHeatGated
    {
        public CompProperties_AbilityImplantJump()
        {
            compClass = typeof(CompAbilityEffect_ImplantJump);
        }
    }

    public class CompAbilityEffect_ImplantJump : CompAbilityEffect_PyrinthHeatGated
    {
    }
}