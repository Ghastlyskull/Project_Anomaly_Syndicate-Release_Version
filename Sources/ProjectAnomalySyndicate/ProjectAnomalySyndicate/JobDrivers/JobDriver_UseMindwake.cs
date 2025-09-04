using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace ProjectAnomalySyndicate
{
    public class JobDriver_UseMindwake : JobDriver
    {
        private const TargetIndex GhoulInd = TargetIndex.A;

        private const TargetIndex ItemInd = TargetIndex.B;

        private const int DurationTicks = 600;

        private Mote warmupMote;

        private Pawn Ghoul => (Pawn)job.GetTarget(TargetIndex.A).Thing;

        private Thing Item => job.GetTarget(TargetIndex.B).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(Ghoul, job, 1, -1, null, errorOnFailed))
            {
                return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.WaitWith(TargetIndex.A,600, true, true, false, TargetIndex.A);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            toil.tickAction = delegate
            {
                CompUsable compUsable = Item.TryGetComp<CompUsable>();
                if (compUsable != null && warmupMote == null && compUsable.Props.warmupMote != null)
                {
                    warmupMote = MoteMaker.MakeAttachedOverlay(Ghoul, compUsable.Props.warmupMote, Vector3.zero);
                }
                warmupMote?.Maintain();
            };
            yield return toil;
            yield return Toils_General.Do(Apply);
        }
        private void Apply()
        {
            CompTargetEffect_Mindwake compTargetEffect_Mindwake = Item.TryGetComp<CompTargetEffect_Mindwake>();

            SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(Ghoul));

            if (compTargetEffect_Mindwake.Props.moteDef != null)
            {
                MoteMaker.MakeAttachedOverlay(Ghoul, compTargetEffect_Mindwake.Props.moteDef, Vector3.zero);
            }
            if (compTargetEffect_Mindwake.Props.addsHediff != null)
            {
                Ghoul.health.AddHediff(compTargetEffect_Mindwake.Props.addsHediff);
            }

            Item.SplitOff(1).Destroy();
        }

    }
}
