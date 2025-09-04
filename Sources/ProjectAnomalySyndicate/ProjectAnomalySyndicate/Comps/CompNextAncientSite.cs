using RimWorld.QuestGen;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompProperties_NextAncientSite : CompProperties
    {
        public CompProperties_NextAncientSite()
        {
            compClass = typeof(CompNextAncientSite);
        }
    }
    public class CompNextAncientSite : ThingComp
    {
        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);
            if (signal != "Hacked")
            {
                return;
            }
            QuestScriptDef questScriptDef = DefOfs.Gha_AncientSite;
            if (questScriptDef != null)
            {
                Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, new Slate());
                if (!quest.hidden && quest.root.sendAvailableLetter)
                {
                    QuestUtility.SendLetterQuestAvailable(quest);
                }
            }
        }        
    }
}
