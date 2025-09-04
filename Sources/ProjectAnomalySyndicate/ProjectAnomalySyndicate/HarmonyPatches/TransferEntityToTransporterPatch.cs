/*using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    [HarmonyPatch]
    public static class TransferEntityToTransporterPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders");
        }
        private static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            if (ModsConfig.RoyaltyActive && ModsConfig.AnomalyActive)
            {
                foreach (LocalTargetInfo item19 in GenUI.TargetsAt(clickPos, TargetingParameters.ForHeldEntity(), thingsOnly: true))
                {
                    Building_HoldingPlatform holdingPlatform;
                    if ((holdingPlatform = item19.Thing as Building_HoldingPlatform) == null)
                    {
                        continue;
                    }
                    Pawn victim = holdingPlatform.HeldPawn;
                    Thing shuttleThing = GenClosest.ClosestThingReachable(holdingPlatform.Position, holdingPlatform.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, IsValidShuttle);
                    if (shuttleThing != null && victim != null && pawn.CanReserveAndReach(holdingPlatform, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
                    {
                        if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
                        {
                            opts.Add(new FloatMenuOption("CannotLoadIntoShuttle".Translate(shuttleThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null));
                        }
                        else if (pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
                        {
                            opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TransferEntityToShuttle".Translate(victim.Label), CarryToShuttleAct), pawn, holdingPlatform));
                        }
                    }
                    void CarryToShuttleAct()
                    {
                        CompShuttle compShuttle = shuttleThing.TryGetComp<CompShuttle>();
                        if (!compShuttle.Transporter.LoadingInProgressOrReadyToLaunch)
                        {
                            TransporterUtility.InitiateLoading(Gen.YieldSingle(compShuttle.Transporter));
                        }
                        Job job2 = JobMaker.MakeJob(DefOfs.TransferEntityToTransporter, item19, shuttleThing, victim);
                        job2.ignoreForbidden = true;
                        job2.count = 1;
                        pawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
                    }
                    bool IsValidShuttle(Thing thing)
                    {
                        return thing.TryGetComp<CompShuttle>()?.IsAllowedNow(victim) ?? false;
                    }
                }
            }


        }
    }
}
*/