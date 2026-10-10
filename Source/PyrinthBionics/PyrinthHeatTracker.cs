using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class PyrinthHeatTracker : IExposable
    {
        public float Current;

        private Pawn pawn;

        public PyrinthHeatTracker(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public int ImplantCount => PyrinthBionicsUtility.CountImplants(pawn);
        public float Max => pawn?.GetStatValue(PyrinthBionicsDefOf.PyrinthBionics_HeatCapacity) ?? 0f;
        public float HeatPerSecond => pawn?.GetStatValue(PyrinthBionicsDefOf.PyrinthBionics_HeatPerSecond) ?? 0f;

        public float Percent => Max > 0f ? Current / Max : 0f;

        public void ExposeData()
        {
            Scribe_Values.Look(ref Current, "current");
        }

        public void SetPawn(Pawn newPawn)
        {
            pawn = newPawn;
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
            var max = Max;

            if (Current > max)
                Current = max;

            if (ImplantCount <= 0)
            {
                Current = 0f;
                return;
            }

            Current += HeatPerSecond / GenTicks.TicksPerRealSecond;

            if (Current > max)
                Current = max;
        }

        public float GetEffectiveHeatPerSecond() => HeatPerSecond;
    }
}