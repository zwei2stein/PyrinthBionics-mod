using System.Collections.Generic;
using Verse;

namespace PyrinthBionics
{
    public class Hediff_PyrinthHeat : HediffWithComps
    {
        private PyrinthHeatTracker tracker;

        public override bool Visible => false;

        public PyrinthHeatTracker Tracker
        {
            get
            {
                if (tracker == null)
                    tracker = new PyrinthHeatTracker(pawn);

                return tracker;
            }
        }

        public override void Tick()
        {
            base.Tick();

            Tracker.Tick();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var gizmo in base.GetGizmos())
                yield return gizmo;

            yield return new Gizmo_PyrinthHeat(Tracker);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            if (tracker == null)
                tracker = new PyrinthHeatTracker(pawn);

            tracker.ExposeData();

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                tracker.SetPawn(pawn);
                tracker.Recalculate();
            }
        }
    }
}