using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace ProjectAnomalySyndicate
{
    // Custom class for the Gha Syndicate tribute collector arrival incident
    public class IncidentWorker_GhaSyndicateTributeCollector : IncidentWorker_TraderCaravanArrival
    {
        // Resolve incident parameters to ensure it uses your custom faction
        protected override bool TryResolveParmsGeneral(IncidentParms parms)
        {
            if (!base.TryResolveParmsGeneral(parms))
            {
                return false;
            }

            // Retrieve the Gha_Syndicate faction
            Faction ghaFaction = Find.FactionManager.FirstFactionOfDef(FactionDef.Named("Gha_Syndicate"));
            if (ghaFaction == null)
            {
                Log.Error("Gha_Syndicate faction not found!");
                return false;
            }

            Map map = (Map)parms.target;
            parms.faction = ghaFaction;

            // Filter TraderKindDefs to those with "TributeCollector" category and match your faction
            parms.traderKind = DefDatabase<TraderKindDef>.AllDefsListForReading
                .Where((TraderKindDef t) => t.category == "TributeCollector" && t.faction == ghaFaction.def)
                .RandomElementByWeight((TraderKindDef t) => this.TraderKindCommonality(t, map, parms.faction));

            return true;
        }

        // Determine if the incident can fire now, checking for the custom faction's presence
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Faction ghaFaction = Find.FactionManager.FirstFactionOfDef(FactionDef.Named("Gha_Syndicate"));
            return base.CanFireNowSub(parms) && ghaFaction != null && this.FactionCanBeGroupSource(ghaFaction, parms, false);
        }

        // Determine the commonality for the trader kind
        protected override float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
        {
            return traderKind.CalculatedCommonality;
        }

        // Custom letter notification for the Gha Syndicate's caravan arrival
        protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
        {
            TaggedString letterLabel = "LetterLabelGhaSyndicateTributeCollectorArrival".Translate().CapitalizeFirst();
            TaggedString letterContent = "LetterGhaSyndicateTributeCollectorArrival".Translate(parms.faction.Named("FACTION")).CapitalizeFirst();
            letterContent += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();

            // Notify player if any of their pawns have relations with the arriving pawns
            PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(
                pawns,
                ref letterLabel,
                ref letterContent,
                "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural),
                true,
                true
            );

            base.SendStandardLetter(letterLabel, letterContent, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
        }
    }
}