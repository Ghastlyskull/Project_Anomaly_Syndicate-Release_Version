using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate.Quests
{
    public class QuestNode_AnomalyCheckSequence : QuestNode
    {
        public List<QuestNode> nodes = new List<QuestNode>();

        protected override void RunInt()
        {

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Run();
            }
        }

        protected override bool TestRunInt(Slate slate)
        {
            if(Find.Anomaly.Level == 0)
            {
                return false;
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (!nodes[i].TestRun(slate))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
