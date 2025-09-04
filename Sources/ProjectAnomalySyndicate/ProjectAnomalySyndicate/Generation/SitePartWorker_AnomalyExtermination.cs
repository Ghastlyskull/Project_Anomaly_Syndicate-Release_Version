using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace ProjectAnomalySyndicate.Generation
{
    public class SitePartWorker_AnomalyExtermination : SitePartWorker
    {
        public override void Init(Site site, SitePart sitePart)
        {
            base.Init(site, sitePart);
            //site.customLabel = sitePart.def.label.Formatted(site.Faction.Named("FACTION"));
        }

       /* public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            return (string)("Hostiles".Translate() + ": " + "Unknown".Translate().CapitalizeFirst()) + ("\n" + "Contains".Translate() + ": " + "Unknown".Translate().CapitalizeFirst());
        }*/

        private const int SpawnRadius = 20;
        public override void PostMapGenerate(Map map)
        {
            Site site = map.Parent as Site;
            PawnGroupKindDef pawnGroup = SyndicateUtility.GetAnomalyGroupKindDefBasedOnMonolithLevel().RandomElement();
            float num = Math.Max(Faction.OfEntities.def.MinPointsToGeneratePawnGroup(pawnGroup), site.ActualThreatPoints);
            List<Pawn> entities = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = pawnGroup,
                points = num,
                faction = Faction.OfEntities
            }).ToList();
            DistressCallUtility.SpawnPawns(map, entities, map.Center, 20);
            if(Find.Anomaly.Level >= 1 && pawnGroup == PawnGroupKindDefOf.Shamblers)
            {
                GameCondition cond = GameConditionMaker.MakeConditionPermanent(GameConditionDefOf.DeathPall);
                map.gameConditionManager.RegisterCondition(cond);
            }
            foreach (Thing allThing in map.listerThings.AllThings)
            {
                if (allThing.def.category == ThingCategory.Item)
                {
                    CompForbiddable compForbiddable = allThing.TryGetComp<CompForbiddable>();
                    if (compForbiddable != null && !compForbiddable.Forbidden)
                    {
                        allThing.SetForbidden(value: true, warnOnFail: false);

                    }
                }

            }

        }

    }
}
