using System.Collections.Generic;
using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class CompProperties_AbilityCauterizeWounds : CompProperties_AbilityPyrinthHeatGated
    {
        public FloatRange heatStrokeSideEffect = new FloatRange(0.2f, 0.4f);

        public CompProperties_AbilityCauterizeWounds()
        {
            compClass = typeof(CompAbilityEffect_CauterizeWounds);
        }
    }

    public class CompAbilityEffect_CauterizeWounds : CompAbilityEffect_PyrinthHeatGated
    {
        private new CompProperties_AbilityCauterizeWounds Props =>
            (CompProperties_AbilityCauterizeWounds)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            var pawn = target.Pawn;
            if (pawn?.health?.hediffSet == null || pawn.Dead)
                return;
            
            if (!Heat.TryConsume(HeatCost))
                return;

            base.Apply(target, dest);

            CauterizeBleedingWounds(pawn);
            ThermalSideEffectUtils.ApplyThermalSideEffect(pawn, Props.heatStrokeSideEffect);

            if (pawn.Spawned)
                FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.MicroSparks);
            if (parent.pawn.Spawned)
                FleckMaker.ThrowMetaIcon(parent.pawn.Position, parent.pawn.Map, FleckDefOf.MicroSparks);
        }

        private void CauterizeBleedingWounds(Pawn targetPawn)
        {
            var burnHediffDef = DamageDefOf.Burn.hediff;

            var hediffs = targetPawn.health.hediffSet.hediffs;

            var toCauterizeInjuries = new List<Hediff_Injury>();
            var toCauterizeStumps = new List<Hediff_MissingPart>();

            for (var i = 0; i < hediffs.Count; i++)
            {
                if (!hediffs[i].Bleeding)
                    continue;

                switch (hediffs[i])
                {
                    case Hediff_Injury injury:
                        toCauterizeInjuries.Add(injury);
                        break;

                    case Hediff_MissingPart missingPart:
                        toCauterizeStumps.Add(missingPart);
                        break;

                    default:
                        if (hediffs[i].TendableNow())
                            hediffs[i].Tended(0.01f, 1f);
                        break;
                }
            }

            foreach (var injury in toCauterizeInjuries)
            {
                var part = injury.Part;
                var severity = injury.Severity;

                if (part != null && targetPawn.health.hediffSet.GetPartHealth(part) <= 0f) continue;

                targetPawn.health.RemoveHediff(injury);

                var burn = HediffMaker.MakeHediff(
                    burnHediffDef,
                    targetPawn,
                    part
                );

                burn.Severity = severity * 0.9f;

                targetPawn.health.AddHediff(burn, part);
            }

            foreach (var stump in toCauterizeStumps)
            {
                stump.lastInjury = burnHediffDef;
                stump.Tended(1f, 1f);
            }
        }

    }
}