using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate
{
    public static class SyndicateUtility
    {

        public static List<PawnKindDef> GetAnomalyPawnKindDefBasedOnMonolithLevel()
        {
            List<PawnKindDef> list = new List<PawnKindDef>() { PawnKindDefOf.ShamblerSwarmer };
            int num = Find.Anomaly.Level;
            if (num >= 1)
            {
                list.Add(PawnKindDefOf.ShamblerGorehulk);
                list.Add(PawnKindDefOf.Metalhorror);
                list.Add(PawnKindDefOf.Chimera);
                list.Add(DefOfs.Devourer);
                list.Add(PawnKindDefOf.Sightstealer);
                list.Add(PawnKindDefOf.Ghoul);
            }
            return list;
        }
        public static List<PawnGroupKindDef> GetAnomalyGroupKindDefBasedOnMonolithLevel()
        {
            List<PawnGroupKindDef> list = new List<PawnGroupKindDef>() { PawnGroupKindDefOf.Shamblers };
            int num = Find.Anomaly.Level;
            if(num >= 1)
            {
                list.Add(PawnGroupKindDefOf.Gorehulks);
                list.Add(PawnGroupKindDefOf.Metalhorrors);
                list.Add(PawnGroupKindDefOf.Chimeras);
                list.Add(PawnGroupKindDefOf.Devourers);
                list.Add(PawnGroupKindDefOf.Fleshbeasts);
                list.Add(PawnGroupKindDefOf.Sightstealers);
            }
            return list;
        }
        public static List<ThingDef> GetBonusItemsBasedOnRank(string rank)
        {
            List<ThingDef> addItems = new List<ThingDef>();
            switch (rank)
            {
                case "Gha_Auxiliary":
                    addItems.Add(DefOfs.Gha_BuildingDataChip);
                    break;
                case "Gha_Operative":
                    addItems.Add(DefOfs.Gha_WeaponDataChip);
                    break;
                case "Gha_Enforcer":
                    addItems.Add(DefOfs.Gha_ArmorDataChip);
                    if (ModsConfig.BiotechActive)
                    {
                        addItems.Add(ThingDefOf.Mechlink);
                    }
                    break;
                case "Gha_Operations_Lieutenant":
                    addItems.Add(DefOfs.Gha_SerumDataChip);
                    break;
                case "Gha_Field_Analyst":
                    addItems.Add(DefOfs.Gha_MechanoidDataChip);
                    if (ModsConfig.BiotechActive)
                    {
                        addItems.Add(ThingDefOf.Mechlink);
                    }
                    break;
                default:
                    break;

            }
            return addItems;
        }
        public static void SendBonusItems(this Map map, List<ThingDef> items, IntVec3 pos)
        {
            List<Thing> things = new List<Thing>();
            foreach(ThingDef item in items)
            {
                things.Add(ThingMaker.MakeThing(item));
            }
            DropPodUtility.DropThingsNear(pos.InBounds(map)? pos : DropCellFinder.TradeDropSpot(map), map, things, 110, canInstaDropDuringInit: false, leaveSlag: false, canRoofPunch: false, forbid: false, allowFogged: true, Find.FactionManager.FirstFactionOfDef(DefOfs.Gha_Syndicate));
            StringBuilder sb = new StringBuilder();
            if(things.Count > 1)
            {
                sb.Append("SyndicateBestowerBonusItemsPlural".Translate());

            }
            else
            {
                sb.Append("SyndicateBestowerBonusItemsSingular".Translate());
            }
            sb.Append(" ");
            sb.Append(string.Join(", ", things.Select(c => c.LabelCap)));
            Messages.Message(sb.ToString(), MessageTypeDefOf.PositiveEvent);
        }
    }
}
