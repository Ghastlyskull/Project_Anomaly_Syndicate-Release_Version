using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace ProjectAnomalySyndicate.Quests
{
    public class QuestsIncidentWorker_FleshbeastsAttackWalkin : IncidentWorker
    {
        private const int MaxIterations = 100;

        private const float FleshbeastPointsFactor = 0.6f;

        private const int AnimalsStayDurationMin = 60000;

        private const int AnimalsStayDurationMax = 120000;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!ModsConfig.AnomalyActive)
            {
                return false;
            }
            Map map = (Map)parms.target;
            IntVec3 result;
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            return RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            //Log.Message("Executing");
            Map map = (Map)parms.target;
            //Log.Message("Got map");
            float num = parms.points;
            //Log.Message("Got points" + num);
            List<Pawn> fleshbeastsForPoints = new List<Pawn>();

            IntVec3 result = parms.spawnCenter;
            if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal))
            {
                return false;
            }
            float num3 = Mathf.Max(num, 50f);
            //Log.Message("Actual points " + num3);
            fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(num3, map, true);
            //Log.Message("Got fleshbeasts " + fleshbeastsForPoints.Count);
            Rot4 rot = Rot4.FromAngleFlat((map.Center - result).AngleFlat);
            for (int i = 0; i < fleshbeastsForPoints.Count; i++)
            {
                Pawn pawn = fleshbeastsForPoints[i];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, map, 10);
                QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot), parms.questTag);
                pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
            }
            SendStandardLetter("FleshbeastsAttackWalkInLabel".Translate(), "FleshbeastsAttackWalkInText".Translate(), LetterDefOf.ThreatBig, parms, fleshbeastsForPoints);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            return true;
        }
    }
}
