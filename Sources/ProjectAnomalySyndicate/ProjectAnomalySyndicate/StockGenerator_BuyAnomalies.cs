using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class StockGenerator_BuyAnomalies : StockGenerator
    {
        // Generates an empty list of things (usually, this would generate pawns/items for sale)
        public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
        {
            return Enumerable.Empty<Thing>();
        }
        // [Left Off on above]
        // Determines if this trader handles a particular ThingDef (e.g., pawns suitable for slavery)
        public override bool HandlesThingDef(ThingDef thingDef)
        {
            // Check if the item is a humanlike pawn with tradeability set
            if (thingDef.tradeability > Tradeability.None && thingDef.race != null)
            {
                //Log.Message(thingDef.defName);
                if (thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.defName == "CreepJoiner")
                {
                    return true;
                }
                if (thingDef.race.IsAnomalyEntity && thingDef.statBases.Any(c => c.stat == StatDefOf.RoyalFavorValue))
                {
                    //Log.Message(thingDef.statBases.Where(c => c.stat == StatDefOf.RoyalFavorValue).First().value);
                    return true;
                }
            }
            return false;


        }

        // Sets tradeability for eligible ThingDefs, allowing only sellable items
        public override Tradeability TradeabilityFor(ThingDef thingDef)
        {
            // Ensure this trader only handles defined ThingDefs with tradeability
            if (!this.HandlesThingDef(thingDef))
            {
                return Tradeability.None;
            }
            return Tradeability.Sellable;
        }
    }
}