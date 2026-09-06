
using HarmonyLib;
using RimWorld;
using Verse;

namespace PyrinthBionics
{

    [HarmonyPatch(typeof(Hediff), nameof(Hediff.ExposeData))]
    public static class Patch_FixHediffAbilityVerbLink
    {
        public static void Postfix(Hediff __instance)
        {
            if (Scribe.mode != LoadSaveMode.PostLoadInit)
                return;

            var abilities = __instance.AllAbilitiesForReading;
            if (abilities == null)
                return;

            foreach (var ability in abilities)
            {
                if (ability?.verb is Verb_CastAbility vca && vca.ability == null)
                {
                    vca.Ability = ability;
                    Log.Message("[PyrinthBionic] patched null ability in verb cast ability " + ability.def.defName);
                }
            }
        }
    }


    [StaticConstructorOnStartup]
    public static class PyrinthBionicsHarmonyBootstrap
    {
        static PyrinthBionicsHarmonyBootstrap()
        {
            new Harmony("zwei2stein.pyrinthbionics").PatchAll();
        }
    }

}