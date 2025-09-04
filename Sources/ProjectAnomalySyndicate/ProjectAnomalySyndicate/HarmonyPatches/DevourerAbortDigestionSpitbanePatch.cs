using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    [HarmonyPatch]
    public static class DevourerAbortDigestionSpitbanePatch
    {
        
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(CompDevourer), nameof(CompDevourer.CompTick));
        }

        private static void Postfix(CompDevourer __instance, int ___ticksDigesting)
        {

            if (__instance.Digesting && __instance.DigestingPawn.health.hediffSet.HasHediff(DefOfs.Spitbane) && ___ticksDigesting == 80)
            {
                __instance.DigestJobFinished();
                __instance.Pawn.stances?.stunner?.StunFor(160, null);
            }
        }
    }
}
