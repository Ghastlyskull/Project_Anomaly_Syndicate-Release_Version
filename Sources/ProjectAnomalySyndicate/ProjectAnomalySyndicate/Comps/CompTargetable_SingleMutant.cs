using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Weapons;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompTargetable_SingleMutant : CompTargetable
    {
        protected override bool PlayerChoosesTarget => true;

        protected override TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                canTargetSubhumans = true,
                canTargetBuildings = false,
                canTargetHumans = true,
                canTargetAnimals = false,
                canTargetBloodfeeders = false,
                canTargetEntities = false,
                canTargetCorpses = false,
                canTargetFires = false,
                canTargetItems = false,
                canTargetLocations = false,
                canTargetMechs = false,
                canTargetPlants = false,
                canTargetSelf = true,
                validator = (c) =>
                {
                    if(c.Thing is Pawn p && p.IsMutant && (p.mutant.Def == MutantDefOf.Ghoul || p.mutant.Def == DefOfs.GhoulWork))
                    {
                        return true;
                    }
                    return false;
                }
            };
        }

        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
        }
    }
}
