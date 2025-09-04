using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.Grammar;
using Verse;
using RimWorld.Planet;

namespace ProjectAnomalySyndicate.Quests
{
    public class QuestNode_HoraxRitualEffect : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> inSignal;

        [NoTranslate]
        public SlateRef<string> tag;

        private const string RootSymbol = "root";

        protected override bool TestRunInt(Slate slate)
        {
            if (!ModsConfig.AnomalyActive)
            {

                return false;
            }
            //Log.Message("Anomaly is active");
            if (!Find.Storyteller.difficulty.allowViolentQuests)
            {

                return false;
            }
           // Log.Message("Violent quests are active");
            if (!slate.Exists("map"))
            {

                return false;
            }
            //Log.Message("map exists");
            return true;
        }

        protected override void RunInt()
        {
            //Log.Message("Running ritual effect");
            Slate slate = QuestGen.slate;
            Map map = QuestGen.slate.Get<Map>("map");
            //Log.Message(map.IsPlayerHome);
            Site site = QuestGen.slate.Get<Site>("site");
            float points = QuestGen.slate.Get("points", 0f);
            points = Math.Max(points, 100f);
            QuestPart_Incident questPart_Incident = new QuestPart_Incident();
            switch (Rand.RangeInclusive(1, 3))
            {
                case 1:
                    questPart_Incident.incident = IncidentDefOf.PitGate;
                    break;
                case 2:
                    questPart_Incident.incident = DefOfs.BloodRain;
                    break;
                case 3:
                    questPart_Incident.incident = DefOfs.DeathPall;
                    break;
            }

            IncidentParms incidentParms = new IncidentParms();
            incidentParms.forced = true;
            incidentParms.target = map;
            incidentParms.points = points;
            incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(tag.GetValue(slate));
            /*            if (!customLetterLabel.GetValue(slate).NullOrEmpty() || customLetterLabelRules.GetValue(slate) != null)
                        {
                            QuestGen.AddTextRequest("root", delegate (string x)
                            {
                                incidentParms.customLetterLabel = x;
                            }, QuestGenUtility.MergeRules(customLetterLabelRules.GetValue(slate), customLetterLabel.GetValue(slate), "root"));
                        }
                        if (!customLetterText.GetValue(slate).NullOrEmpty() || customLetterTextRules.GetValue(slate) != null)
                        {
                            QuestGen.AddTextRequest("root", delegate (string x)
                            {
                                incidentParms.customLetterText = x;
                            }, QuestGenUtility.MergeRules(customLetterTextRules.GetValue(slate), customLetterText.GetValue(slate), "root"));
                        }*/
            questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
            questPart_Incident.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal");
            QuestGen.quest.AddPart(questPart_Incident);
        }
    }

}

