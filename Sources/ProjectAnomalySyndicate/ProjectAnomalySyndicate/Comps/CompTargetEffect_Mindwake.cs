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
    public class CompProperties_TargetEffectMindwake : CompProperties
    {
        public ThingDef moteDef;

        public bool withSideEffects = true;

        public HediffDef addsHediff;

        public CompProperties_TargetEffectMindwake()
        {
            compClass = typeof(CompTargetEffect_Mindwake);
        }
    }
    public class CompTargetEffect_Mindwake : CompTargetEffect
    {
        public CompProperties_TargetEffectMindwake Props => (CompProperties_TargetEffectMindwake)props;

        public override void DoEffectOn(Pawn user, Thing target)
        {
            if (user.IsColonistPlayerControlled)
            {
                Job job = JobMaker.MakeJob(DefOfs.UseMindWake, target, parent);
                job.count = 1;
                job.playerForced = true;
                user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            }
        }
    }
}
