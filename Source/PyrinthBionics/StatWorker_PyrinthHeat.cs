using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class StatWorker_PyrinthHeat : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!base.ShouldShowFor(req) || !(req.Thing is Pawn))
                return false;

            return !Mathf.Approximately(GetValue(req), stat.defaultBaseValue);
        }
    }
}