using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    [HarmonyPatch]
    public static class HediffCompInvisibilityForcedVisiblePatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(HediffComp_Invisibility), "get_ForcedVisible");
        }
        private static void Postfix(ref bool __result, HediffComp_Invisibility __instance)
        {
            //Log.Message("Entering postfix");
            if (!__result)
            {
                // Log.Message("Result is false checking for Pawns");
                //Log.Message(__instance.Pawn.Label);
                if (__instance.Pawn.Map != null)
                {
                    foreach (Pawn item in __instance.Pawn.Map.mapPawns.AllPawnsSpawned.ToList())
                    {

                        //Log.Message(item.Label);
                        if (item.Spawned && item != __instance.Pawn && item.apparel != null)
                        {
                            //Log.Message("Pawn spawned, checking apparel");
                            float radius = 0f;
                            IEnumerable<Apparel> l = item.apparel.WornApparel.Where(c => c.def.HasModExtension<InvisibilityDisruptorModExtension>());
                            if (l.Any())
                            {
                                foreach (Apparel appa in l)
                                {
                                    float potRadius = appa.def.GetModExtension<InvisibilityDisruptorModExtension>().radius;
                                    if (radius < potRadius)
                                    {
                                        radius = potRadius;
                                    }
                                }
                                if (radius > 0f && item.Position.InHorDistOf(__instance.Pawn.Position, radius) && GenSight.LineOfSightToThing(__instance.Pawn.Position, item, __instance.Pawn.Map))
                                {
                                    __result = true;
                                }
                            }

                        }


                    }
                }

            }

        }


    }
}
