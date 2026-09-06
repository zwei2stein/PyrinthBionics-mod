using RimWorld;
using Verse;

namespace PyrinthBionics
{
    public class ThoughtWorker_PyromaniacImplants : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p?.story?.traits == null || !p.story.traits.HasTrait(PyrinthBionicsDefOf.Pyromaniac))
                return ThoughtState.Inactive;

            var implantCount = PyrinthBionicsUtility.CountImplants(p);
            if (implantCount <= 0) return ThoughtState.Inactive;

            var stageIndex = implantCount - 1;
            var maxStageIndex = def.stages.Count - 1;
            if (stageIndex > maxStageIndex) stageIndex = maxStageIndex;

            return ThoughtState.ActiveAtStage(stageIndex);
        }
    }
}