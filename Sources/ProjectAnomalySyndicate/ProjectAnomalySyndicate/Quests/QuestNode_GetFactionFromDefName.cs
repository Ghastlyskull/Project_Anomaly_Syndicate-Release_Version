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
    public class QuestNode_GetFactionFromDefName : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> storeAs;

        public SlateRef<string> defName;

        protected override bool TestRunInt(Slate slate)
        {
            SetVars(slate);
            return true;
        }

        protected override void RunInt()
        {
            SetVars(QuestGen.slate);
        }

        private void SetVars(Slate slate)
        {
            string val = defName.GetValue(slate);
            Find.FactionManager.GetFactions(true, true, true).Where(c => c.def.defName == val).TryRandomElement(out Faction faction);
            slate.Set(storeAs.GetValue(slate), faction);
        }
    }
}
