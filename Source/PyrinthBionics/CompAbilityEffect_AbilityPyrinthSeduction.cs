using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class CompProperties_AbilityPyrinthSeduction : CompProperties_AbilityPyrinthHeatGated
    {
        public FloatRange heatStrokeSideEffect = new FloatRange(0.2f, 0.4f);
        
        public IntRange prisonerResistanceDecrease = new IntRange(1, 5);
        public IntRange prisonerWillDecrease = new IntRange(1, 5);
        public FloatRange slaveSuppressionIncrease = new FloatRange(0.1f, 0.3f);
        public List<TraitDef> enhancingTraits = new List<TraitDef>();
        public List<GeneDef> backfiringGenes = new List<GeneDef>();
        
        public CompProperties_AbilityPyrinthSeduction()
        {
            compClass = typeof(Comp_AbilityPyrinthSeduction);
        }
    }

    public class Comp_AbilityPyrinthSeduction : CompAbilityEffect_PyrinthHeatGated
    {
        private new CompProperties_AbilityPyrinthSeduction Props =>
            (CompProperties_AbilityPyrinthSeduction)props;

        public override bool GizmoDisabled(out string reason)
        {
            if (StatDefOf.NegotiationAbility.Worker.IsDisabledFor(parent.pawn))
            {
                reason = "PyrinthBionics_PyrinthSeduction_MustHaveNegotiationAbility".Translate(
                    parent.pawn.Named("USER")
                );
                return true;
            }

            string baseReason;
            var baseValue = base.GizmoDisabled(out baseReason);
            reason = baseReason;
            return baseValue;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            var pawn = target.Pawn;
            if (pawn?.health?.hediffSet == null || pawn.Dead)
                return;
            
            if (!Heat.TryConsume(HeatCost))
                return;

            base.Apply(target, dest);
            
            Seduce(pawn);
            ThermalSideEffectUtils.ApplyThermalSideEffect(pawn, Props.heatStrokeSideEffect);

            if (pawn.Spawned)
                FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.MicroSparks);
            if (parent.pawn.Spawned)
                FleckMaker.ThrowMetaIcon(parent.pawn.Position, parent.pawn.Map, FleckDefOf.MicroSparks);
        }

        private void Seduce(Pawn targetPawn)
        {
            
            var effectStrength = parent.pawn.GetStatValue(StatDefOf.NegotiationAbility);
            
            if (Props.enhancingTraits != null && targetPawn.story?.traits != null)
            {
                foreach (var trait in Props.enhancingTraits)
                {
                    if (targetPawn.story.traits.HasTrait(trait))
                    {
                        effectStrength *= 2f;
                        break;
                    }
                }
            }

            if (ModLister.BiotechInstalled && Props.backfiringGenes != null && targetPawn.genes != null)
            {
                foreach (var backfiringGene in Props.backfiringGenes)
                {
                    if (targetPawn.genes.HasActiveGene(backfiringGene))
                    {
                        effectStrength *= -1f;
                        break;
                    }
                }
            }

            // Prisoner effects.
            if (targetPawn.IsPrisoner)
            {
                targetPawn.guest.resistance = Mathf.Clamp(
                    targetPawn.guest.resistance - Props.prisonerResistanceDecrease.RandomInRange * effectStrength,
                    0.0f,
                    1.0f);

                // ReduceWill is an Ideology interaction.
                if (ModLister.IdeologyInstalled &&
                    targetPawn.guest != null && targetPawn.guest.will > 0.0)
                {
                    targetPawn.guest.will = Mathf.Clamp(
                        targetPawn.guest.will - Props.prisonerWillDecrease.RandomInRange * effectStrength,
                        0.0f,
                        1.0f);
                }
            }

            // Slave suppression is Ideology-only.
            if (ModLister.IdeologyInstalled && targetPawn.IsSlave)
            {
                ApplySlaveSuppression(targetPawn, effectStrength);
            }

            int stageIndex;
            
            if (effectStrength < 0f)
                stageIndex = 0; // backfire
            else if (effectStrength > 1f)
                stageIndex = 2; // great success
            else
                stageIndex = 1; // success

            // Add social memory
            Thought_Memory thought = ThoughtMaker.MakeThought(
                PyrinthBionicsDefOf.PyrinthBionics_Seduction,
                stageIndex
            );
            targetPawn.needs.mood.thoughts.memories.TryGainMemory(thought, parent.pawn);
        }
        
        private void ApplySlaveSuppression(Pawn targetPawn, float effectStrength)
        {
            if (targetPawn.needs == null ||
                !targetPawn.needs.TryGetNeed<Need_Suppression>(
                    out Need_Suppression suppression))
            {
                return;
            }

            SlaveRebellionUtility.IncrementSuppression(
                suppression,
                parent.pawn,
                targetPawn,
                Props.slaveSuppressionIncrease.RandomInRange * effectStrength
            );
        }

    }
}