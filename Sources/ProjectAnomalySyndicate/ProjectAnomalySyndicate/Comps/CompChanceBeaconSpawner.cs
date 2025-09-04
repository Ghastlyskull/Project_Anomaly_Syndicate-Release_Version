using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompProperties_ChanceBeaconSpawner : CompProperties
    {
        public float chance = 0.5f;
        public CompProperties_ChanceBeaconSpawner()
        {
            compClass = typeof(CompChanceBeaconSpawner);
        }
    }

    public class CompChanceBeaconSpawner : ThingComp
    {
        public CompProperties_ChanceBeaconSpawner Props => (CompProperties_ChanceBeaconSpawner)props;

        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(500))
            {
                Map map = parent.Map;
                if (map != null && map.mapPawns.FreeColonistsSpawnedCount > 0)
                {
                    if (Rand.Chance(Props.chance))
                    {
                        Thing thing = GenSpawn.Spawn(DefOfs.Gha_CrashSiteBeacon, parent.Position, parent.Map);
                        thing.stackCount = 1;
                    }
                    parent.Destroy();
                }
            }
        }

    }
}
