using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using System.ComponentModel;

namespace ProjectAnomalySyndicate
{
    public class JobDriver_TransferEntityToTransporter : JobDriver
    {
        private const TargetIndex SourceHolderIndex = TargetIndex.A;

        private const TargetIndex DestShuttleIndex = TargetIndex.B;

        private const TargetIndex TakeeIndex = TargetIndex.C;

        private Thing Takee => job.GetTarget(TargetIndex.C).Thing;

        private CompEntityHolder SourceHolder => job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();

        private Thing DestShuttle => job.GetTarget(TargetIndex.B).Thing;
        public Thing ThingToCarry => (Thing)job.GetTarget(TargetIndex.A);


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed) && pawn.Reserve(SourceHolder.parent, job, 1, -1, null, errorOnFailed))
            {
                return pawn.Reserve(DestShuttle, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.C);
            this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
            this.FailOn(delegate
            {
                Thing thing = GetActor().jobs.curJob.GetTarget(TargetIndex.B).Thing;
                if (thing == null)
                {
                    return true;
                }
                ThingOwner thingOwner = DestShuttle.TryGetInnerInteractableThingOwner();
                if (thingOwner != null && !thingOwner.CanAcceptAnyOf(ThingToCarry))
                {
                    return true;
                }
                return (DestShuttle is IHaulDestination haulDestination && !haulDestination.Accepts(ThingToCarry)) ? true : false;
            });
            this.FailOn(() => TransporterUtility.WasLoadingCanceled(DestShuttle));
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return Toils_General.Do(delegate
            {
                Takee.TryGetComp<CompHoldingPlatformTarget>()?.Notify_ReleasedFromPlatform();
                SourceHolder.EjectContents();
            }).FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.C);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A);
        }
    }
}