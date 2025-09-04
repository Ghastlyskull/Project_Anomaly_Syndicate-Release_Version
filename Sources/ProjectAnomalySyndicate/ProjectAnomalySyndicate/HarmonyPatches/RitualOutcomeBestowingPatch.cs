using HarmonyLib;
using RimWorld.QuestGen;
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
    public static class RitualOutcomeBestowingPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(RitualOutcomeEffectWorker_Bestowing), nameof(RitualOutcomeEffectWorker_Bestowing.Apply));
        }
        private static void Postfix(LordJob_Ritual jobRitual)
        {
            if (jobRitual is LordJob_BestowingCeremony { target: { } target, bestower: { } bestower } && bestower.Faction.def == DefOfs.Gha_Syndicate)
            {
                List<ThingDef> list = SyndicateUtility.GetBonusItemsBasedOnRank(target.royalty.GetCurrentTitleInFaction(bestower.Faction).def.defName);
                if(list.Count > 0)
                {
                    target.Map.SendBonusItems(list, bestower.Position);
                }
                
            }
        }
    }
}
