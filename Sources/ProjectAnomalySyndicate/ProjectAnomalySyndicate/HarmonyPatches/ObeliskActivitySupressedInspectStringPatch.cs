using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    [HarmonyPatch]
    public static class ObeliskActivitySupressedInspectStringPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(CompActivity), nameof(CompActivity.CompInspectStringExtra));
        }

        private static void Postfix(ref string __result, CompActivity __instance)
        {
            if (__instance.Props.workerClass == typeof(ObeliskActivityWorker) && !__instance.parent.TryGetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading.Empty())
            {
                __result += "\n" + "BeingSuppressed".Translate();
            }
        }
    }
}
