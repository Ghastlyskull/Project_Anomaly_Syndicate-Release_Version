using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate.Quests
{
    public class QuestNode_GetHediffFromDefName : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> defName;

        public SlateRef<string> storeAs;
        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }
        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            slate.Set(storeAs.GetValue(slate), DefDatabase<HediffDef>.GetNamed(defName.GetValue(slate)));
        }
    }
}
