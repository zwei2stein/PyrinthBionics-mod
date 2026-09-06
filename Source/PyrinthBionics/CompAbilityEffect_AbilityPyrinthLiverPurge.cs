using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class CompProperties_AbilityPyrinthLiverPurge : CompProperties_AbilityPyrinthHeatGated
    {
        public List<HediffDef> removedHediffs = new List<HediffDef>();
        public List<HediffDef> severityDecreasedHediffs = new List<HediffDef>();
        public FloatRange severityDecrease = new FloatRange(0.2f, 0.4f);
        public FloatRange heatStrokeSideEffect = new FloatRange(0.2f, 0.4f);

        public FloatRange burnSeverityRange = new FloatRange(1f, 4f);

        public CompProperties_AbilityPyrinthLiverPurge()
        {
            compClass = typeof(CompAbilityEffect_AbilityPyrinthLiverPurge);
        }
    }

    public class CompAbilityEffect_AbilityPyrinthLiverPurge : CompAbilityEffect_PyrinthHeatGated
    {
        private new CompProperties_AbilityPyrinthLiverPurge Props =>
            (CompProperties_AbilityPyrinthLiverPurge)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (!Heat.TryConsume(HeatCost))
                return;

            base.Apply(target, dest);

            PyrinthLiverPurge(parent.pawn);

            if (parent.pawn.Spawned)
                FleckMaker.ThrowMetaIcon(parent.pawn.Position, parent.pawn.Map, FleckDefOf.MicroSparks);
        }

        private void PyrinthLiverPurge(Pawn pawn)
        {
            var affectedCount = 0;
            var hediffs = pawn.health.hediffSet.hediffs;

            if (!Props.removedHediffs.NullOrEmpty())
            {
                var toRemove = new List<Hediff>();
                foreach (var hediff in hediffs)
                {
                    if (Props.removedHediffs.Contains(hediff.def))
                        toRemove.Add(hediff);
                }

                foreach (var hediff in toRemove)
                {
                    pawn.health.RemoveHediff(hediff);
                    affectedCount++;
                }
            }

            if (!Props.severityDecreasedHediffs.NullOrEmpty())
            {
                var toDecrease = new List<Hediff>();
                foreach (var hediff in hediffs)
                {
                    if (Props.severityDecreasedHediffs.Contains(hediff.def))
                        toDecrease.Add(hediff);
                }

                foreach (var hediff in toDecrease)
                {
                    hediff.Severity = Mathf.Max(0.01f, hediff.Severity - Props.severityDecrease.RandomInRange);
                    affectedCount++;
                }
            }

            ThermalSideEffectUtils.ApplyThermalSideEffect(pawn, Props.heatStrokeSideEffect);
            
            // Trigger Metal Horror if infection is active
            if (ModsConfig.AnomalyActive) 
                MetalhorrorUtility.TryEmerge(pawn,"PyrinthBionics_PyrinthLiverPurge_Metalhorror".Translate(
                    pawn.Named("INFECTED")
                    ));
            
            int burnCount = Rand.RangeInclusive(0, affectedCount);
            for (var i = 0; i < burnCount; i++)
            {
                var part = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Burn);
                if (part == null)
                    continue;

                var burn = HediffMaker.MakeHediff(DamageDefOf.Burn.hediff, pawn, part);
                burn.Severity = Props.burnSeverityRange.RandomInRange;
                pawn.health.AddHediff(burn, part);
            }
        }
    }
}
