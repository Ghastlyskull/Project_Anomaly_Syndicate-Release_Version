using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompProperties_RandomAnomalySpawner : CompProperties
    {
        public float chance = 0.5f;
        public CompProperties_RandomAnomalySpawner()
        {
            compClass = typeof(CompRandomAnomalySpawner);
        }
    }

    public class CompRandomAnomalySpawner : ThingComp
    {

        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(500))
            {
                Map map = parent.Map;
                if (map != null && map.mapPawns.FreeColonistsSpawnedCount > 0)
                {
                    PawnKindDef def = SyndicateUtility.GetAnomalyPawnKindDefBasedOnMonolithLevel().RandomElement();
                    Pawn pawn = PawnGenerator.GeneratePawn(def, Faction.OfEntities);
                    GenSpawn.Spawn(pawn, parent.Position, map);
                    parent.Destroy();
                }
            }
        }

    }
}
