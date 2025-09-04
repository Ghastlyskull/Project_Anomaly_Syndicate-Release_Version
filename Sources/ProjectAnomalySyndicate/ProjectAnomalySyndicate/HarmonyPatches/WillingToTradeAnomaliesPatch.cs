using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
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
    public static class WillingToTradeAnomaliesPawnPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Pawn_TraderTracker), nameof(Pawn_TraderTracker.ColonyThingsWillingToBuy));
        }

        private static void Postfix(ref IEnumerable<Thing> __result, Pawn_TraderTracker __instance, Pawn ___pawn)
        {
            List<Thing> list = __result.ToList();
            if (ModsConfig.AnomalyActive)
            {
                List<Building> list1 = ___pawn.Map.listerBuildings.allBuildingsColonist.Where(c=>c.HasComp<CompEntityHolderPlatform>()).ToList();
                foreach (Building item in list1)
                {
                    //Log.Message(item.Label);
                    CompEntityHolderPlatform comp = item.GetComp<CompEntityHolderPlatform>();
                    if (comp.HeldPawn != null && comp.HeldPawn.RoyalFavorValue > 0)
                    {
                        //Log.Message(comp.HeldPawn.Label);
                        list.Add(comp.HeldPawn);
                    }
                }
            }
            __result = list;
        }
    }
    [HarmonyPatch]
    public static class WillingToTradeAnomaliesTradeDealPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(TradeDeal), "InSellablePosition");
        }

        private static void Postfix(ref bool __result, TradeDeal __instance, Thing t, out string reason)
        {
            //Log.Message(t.ParentHolder);
            if (!__result && !t.Spawned && ModsConfig.AnomalyActive && t.holdingOwner != null && ((Pawn)t).health.capacities.CanBeAwake && !((Pawn)t).DeadOrDowned)
            {
                __result = true;
            }
            reason = null;
        }
    }

}
