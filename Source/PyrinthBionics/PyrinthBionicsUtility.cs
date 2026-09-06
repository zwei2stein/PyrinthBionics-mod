using System.Collections.Generic;
using System.Globalization;
using RimWorld;
using UnityEngine;
using Verse;

namespace PyrinthBionics
{
    public class PyrinthBionicsImplantExtension : DefModExtension
    {
        public float heatCapacity = 10f;
        public float heatPerSecond = 1f;
    }
    
    public readonly struct PyrinthImplantTotals
    {
        public readonly int Count;
        public readonly float HeatCapacity;
        public readonly float HeatPerSecond;

        public PyrinthImplantTotals(int count, float heatCapacity, float heatPerSecond)
        {
            Count = count;
            HeatCapacity = heatCapacity;
            HeatPerSecond = heatPerSecond;
        }
    }

    public static class PyrinthBionicsUtility
    {
        public static PyrinthImplantTotals GetImplantTotals(Pawn pawn)
        {
            var hediffs = pawn?.health?.hediffSet?.hediffs;
            if (hediffs == null)
                return default;

            var count = 0;
            var heatCapacity = 0f;
            var heatPerSecond = 0f;

            for (var i = 0; i < hediffs.Count; i++)
            {
                var ext = hediffs[i].def.GetModExtension<PyrinthBionicsImplantExtension>();
                if (ext == null)
                    continue;

                count++;
                heatCapacity += ext.heatCapacity;
                heatPerSecond += ext.heatPerSecond;
            }

            return new PyrinthImplantTotals(count, heatCapacity, heatPerSecond);
        }

        public static int CountImplants(Pawn pawn)
        {
            return GetImplantTotals(pawn).Count;
        }

        public static IEnumerable<StatDrawEntry> HeatStatEntries(PyrinthBionicsImplantExtension ext)
        {
            if (ext == null)
                yield break;

            yield return new StatDrawEntry(
                PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                "PyrinthBionics_StatEntry_HeatCapacityLabel".Translate(),
                "+" + Mathf.FloorToInt(ext.heatCapacity).ToString("F0"),
                "PyrinthBionics_StatEntry_HeatCapacityDesc".Translate(),
                1000);

            yield return new StatDrawEntry(
                PyrinthBionicsDefOf.PyrinthBionics_StatCategory,
                "PyrinthBionics_StatEntry_HeatPerSecondLabel".Translate(),
                "PyrinthBionics_Gizmo_Gauge_HeatPS_Label".Translate(
                    ext.heatPerSecond.ToString("F1", CultureInfo.InvariantCulture).Named("HEATPS")
                    ),
                "PyrinthBionics_StatEntry_HeatPerSecondDesc".Translate(),
                999);
        }
    }
}