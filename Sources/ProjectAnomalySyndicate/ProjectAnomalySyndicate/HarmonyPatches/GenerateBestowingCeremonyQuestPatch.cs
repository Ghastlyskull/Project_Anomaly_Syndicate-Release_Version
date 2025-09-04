using RimWorld.QuestGen;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using System.Reflection;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    [HarmonyPatch]
    public static class GenerateBestowingCeremonyQuestPatch 
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(RoyalTitleUtility), nameof(RoyalTitleUtility.GenerateBestowingCeremonyQuest));
        }
        private static bool Prefix(Pawn pawn, Faction faction)
        {
            if (faction.def == DefOfs.Gha_Syndicate)
            {
                Slate slate = new Slate();
                slate.Set("titleHolder", pawn);
                slate.Set("bestowingFaction", faction);
                if (DefOfs.Gha_BestowingCeremony.CanRun(slate, pawn.MapHeld))
                {
                    Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(DefOfs.Gha_BestowingCeremony, slate);
                    if (quest.root.sendAvailableLetter)
                    {
                        QuestUtility.SendLetterQuestAvailable(quest);
                    }
                }
                return false;
            }
            return true;
        }
    }
}
