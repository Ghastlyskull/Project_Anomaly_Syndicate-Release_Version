using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompProperties_MindWake : HediffCompProperties
    {
        public CompProperties_MindWake()
        {
            compClass = typeof(CompMindWake);
        }
    }
    public class CompMindWake : HediffComp
    {
        HairDef hair = null;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            hair = Pawn.story.hairDef;
            Pawn.mutant.Revert();
            MutantUtility.SetPawnAsMutantInstantly(Pawn, DefOfs.GhoulWork);
            Pawn.story.hairDef = hair;
            Pawn.Drawer.renderer.SetAllGraphicsDirty();

        }
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            hair = Pawn.story.hairDef;
            Pawn.mutant.Revert();
            MutantUtility.SetPawnAsMutantInstantly(Pawn, MutantDefOf.Ghoul);
            Pawn.story.hairDef = hair;
            Pawn.Drawer.renderer.SetAllGraphicsDirty();

        }
    }
}
