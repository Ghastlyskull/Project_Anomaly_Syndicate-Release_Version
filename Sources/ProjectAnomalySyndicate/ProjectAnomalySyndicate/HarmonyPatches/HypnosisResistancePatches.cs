using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;
using UnityEngine;
using Unity.Jobs;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    public static class HypnosisResistanceHelper
    {
        public static int CalculateHypnosisResistance(int originalValue, Pawn pawn)
        {
            //Log.Message("Original value " + originalValue);
            int result = originalValue;
            IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(c => c.def.HasModExtension<HypnosisResistanceModExtension>());
            if (hediffs.Any())
            {
                foreach (Hediff hediff in hediffs)
                {
                    result += (int)(originalValue * hediff.def.GetModExtension<HypnosisResistanceModExtension>().percentageIncrease);
                }
            }
            //Log.Message("Altered value " + result);
            return result;
        }
    }
    [HarmonyPatch]
    public static class HypnosisResistanceRevenantPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(JobDriver_RevenantAttack), "get_HypnotizeDurationTicks");
        }
        private static void Postfix(ref int __result, JobDriver_RevenantAttack __instance)
        {
            __result = HypnosisResistanceHelper.CalculateHypnosisResistance(__result, (Pawn)__instance.job.targetA.Thing);
        }
    }
    [HarmonyPatch]
    public static class HypnosisResistanceUnnaturalCorpsePatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(UnnaturalCorpseTracker), "AwakeningTick");
        }
        private static void Postfix(UnnaturalCorpseTracker __instance, ref int ___ticksToKill)
        {
            ___ticksToKill = HypnosisResistanceHelper.CalculateHypnosisResistance(___ticksToKill, __instance.Haunted);
        }
    }

}
