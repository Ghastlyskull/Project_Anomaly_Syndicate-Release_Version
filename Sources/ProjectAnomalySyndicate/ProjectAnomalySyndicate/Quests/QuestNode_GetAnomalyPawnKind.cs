using RimWorld.QuestGen;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ProjectAnomalySyndicate.Quests
{
    public class QuestNode_GetAnomalyPawnKind : QuestNode
    {

        [NoTranslate]
        public SlateRef<string> storeAs;

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
            PawnKindDef def = SyndicateUtility.GetAnomalyPawnKindDefBasedOnMonolithLevel().RandomElement();
            slate.Set(storeAs.GetValue(slate), def);

        }
    }
}
