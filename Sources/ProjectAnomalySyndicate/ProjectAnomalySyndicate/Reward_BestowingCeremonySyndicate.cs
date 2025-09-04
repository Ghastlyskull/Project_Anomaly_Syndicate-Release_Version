using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace ProjectAnomalySyndicate
{
    [StaticConstructorOnStartup]
    public class Reward_BestowingCeremonySyndicate : Reward_BestowingCeremony
    {
        private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer");

        public List<ThingDef> bonusItems;
        public override IEnumerable<GenUI.AnonymousStackElement> StackElements
        {
            get
            {
                if (givePsylink)
                {
                    yield return QuestPartUtility.GetStandardRewardStackElement("Reward_BestowingCeremony_Label".Translate(), IconPsylink, () => GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate
                    {
                        Find.WindowStack.Add(new Dialog_InfoCard(HediffDefOf.PsychicAmplifier));
                    });
                }

                yield return QuestPartUtility.GetStandardRewardStackElement("Reward_Title_Label".Translate(titleName.Named("TITLE")), awardingFaction.def.FactionIcon, () => "Reward_Title".Translate(targetPawnName.Named("PAWNNAME"), titleName.Named("TITLE"), awardingFaction.Named("FACTION")).Resolve() + ".", delegate
                {
                    if (royalTitle != null)
                    {
                        Find.WindowStack.Add(new Dialog_InfoCard(royalTitle, awardingFaction));
                    }
                });
                foreach (ThingDef item in bonusItems)
                {
                    yield return QuestPartUtility.GetStandardRewardStackElement(item.LabelCap, item.uiIcon, () => item.description, delegate
                    {
                        Find.WindowStack.Add(new Dialog_InfoCard(item));

                    });
                }
            }
        }
    }
}