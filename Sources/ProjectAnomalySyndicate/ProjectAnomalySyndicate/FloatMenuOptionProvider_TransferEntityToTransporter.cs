using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class FloatMenuOptionProvider_TransferEntityToTransporter : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;

        protected override bool Undrafted => true;

        protected override bool Multiselect => false;

        protected override bool RequiresManipulation => true;

        protected override bool AppliesInt(FloatMenuContext context)
        {
            return ModsConfig.AnomalyActive;
        }

        protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
        {
            if (!clickedThing.TryGetComp(out CompEntityHolder holdingPlatform) || holdingPlatform.HeldPawn == null || !context.FirstSelectedPawn.CanReserveAndReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
            {
                return null;
            }
            Pawn pawn = context.FirstSelectedPawn;
            Pawn victim = holdingPlatform.HeldPawn;
            Thing shuttleThing = GenClosest.ClosestThingReachable(holdingPlatform.parent.Position, holdingPlatform.parent.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, IsValidShuttle);
            if(shuttleThing != null && victim != null && pawn.CanReserveAndReach(holdingPlatform.parent, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
            {
                if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
                {
                    return new FloatMenuOption("CannotLoadIntoShuttle".Translate(shuttleThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null);
                }
                else if (pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
                {
                    return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TransferEntityToShuttle".Translate(victim.Label), CarryToShuttleAct), pawn, clickedThing);
                }
            }
            return null;
            void CarryToShuttleAct()
            {
                CompShuttle compShuttle = shuttleThing.TryGetComp<CompShuttle>();
                if (!compShuttle.Transporter.LoadingInProgressOrReadyToLaunch)
                {
                    TransporterUtility.InitiateLoading(Gen.YieldSingle(compShuttle.Transporter));
                }
                Job job2 = JobMaker.MakeJob(DefOfs.TransferEntityToTransporter, clickedThing, shuttleThing, victim);
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

