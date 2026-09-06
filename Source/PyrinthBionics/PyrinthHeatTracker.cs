using System.Collections.Generic;
using Verse;

namespace PyrinthBionics
{
    public class PyrinthHeatTracker : IExposable
    {
        public const float BaseCapacity = 50f;

        public float Current;

        private Pawn pawn;

        public PyrinthHeatTracker(Pawn pawn)
        {
            this.pawn = pawn;
            Recalculate();
        }
        
        public int ImplantCount { get; private set; }
        public float Max { get; private set; }
        public float HeatPerSecond { get; private set; }

        public float Percent => Max > 0f ? Current / Max : 0f;

        public void ExposeData()
        {
            Scribe_Values.Look(ref Current, "current");
        }
        
        public void SetPawn(Pawn newPawn)
        {
            pawn = newPawn;
        }
        
        public void Recalculate()
        {
            var totals = PyrinthBionicsUtility.GetImplantTotals(pawn);
            ImplantCount = totals.Count;
            Max = BaseCapacity + totals.HeatCapacity;
            HeatPerSecond = totals.HeatPerSecond;
        }

        public bool CanConsume(float amount)
        {
            return Current >= amount;
        }

        public bool TryConsume(float amount)
        {
            if (!CanConsume(amount))
                return false;

            Current -= amount;
            return true;
        }

        public void Tick()
        {
            if (Current > Max)
                Current = Max;

            if (ImplantCount <= 0)
            {
                Current = 0f;
                return;
            }

            Current += GetEffectiveHeatPerSecond() / GenTicks.TicksPerRealSecond;

            if (Current > Max)
                Current = Max;
        }

        public float GetEffectiveHeatPerSecond()
        {
            var result = HeatPerSecond;

            if (pawn?.health?.hediffSet == null)
                return result;

            foreach (var t in pawn.health.hediffSet.hediffs)
            {
                var comp = t.TryGetComp<HediffComp_PyrinthHeatBoost>();

                if (comp != null)
                    result += comp.HeatPerSecondBonus;
            }

            return result;
        }
        
    }
}