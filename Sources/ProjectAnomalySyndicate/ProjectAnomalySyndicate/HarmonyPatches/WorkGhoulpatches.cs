using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ProjectAnomalySyndicate.HarmonyPatches
{
    #region GhoulTabs
    [HarmonyPatch]
    public static class WorkTabGhoulsPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(MainTabWindow_Work), "get_Pawns");
        }
        public static void Postfix(ref IEnumerable<Pawn> __result, MainTabWindow_Schedule __instance)
        {
            __result = __result.Concat(Find.CurrentMap.mapPawns.SpawnedColonySubhumansPlayerControlled.Where(c => c.health.hediffSet.hediffs.Any(x => x.def.HasComp(typeof(CompMindWake)))));
        }
    }
    [HarmonyPatch]
    public static class AssignTabGhoulsPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(MainTabWindow_Assign), "get_Pawns");
        }
        public static void Postfix(ref IEnumerable<Pawn> __result, MainTabWindow_Assign __instance)
        {
            __result = __result.Concat(Find.CurrentMap.mapPawns.SpawnedColonySubhumansPlayerControlled.Where(c => c.health.hediffSet.hediffs.Any(x => x.def.HasComp(typeof(CompMindWake)))));
        }
    }
    #endregion

    #region FloatMenuProviderPatch
    [HarmonyPatch]
    public static class FloatMenuOptionProviderGhoulMindWakePatch
    {
        public static bool HasMindWake(Pawn p)
        {
            return p.health.hediffSet.hediffs.Any(x => x.def.HasComp(typeof(CompMindWake)));
        }
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(FloatMenuOptionProvider), "SelectedPawnValid");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool doCount = false;
            bool doWork = false;
            int count = 0;
            object target = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (doCount)
                {
                    count++;
                    if(count == 1)
                    {
                        target = instruction.operand;
                    }
                    if(count == 2)
                    {
                        doCount = false;
                        doWork = true;
                    }
                }
                if (doWork)
                {
                    doWork = false;
                    yield return new(OpCodes.Ldarg_1);
                    yield return new(OpCodes.Call, AccessTools.Method(typeof(FloatMenuOptionProviderGhoulMindWakePatch), nameof(HasMindWake)));
                    yield return new(OpCodes.Brtrue, target);
                }
                if (instruction.Calls(AccessTools.Method(typeof(Pawn), "get_IsMutant")))
                {
                    doCount = true;
                }
                yield return instruction;
            }
        }
    }
    #endregion

    #region ApparelImplantCheck
    [HarmonyPatch]
    public static class ApparelImplantCheckPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(EquipmentUtility), "CanEquip", new Type[] { typeof(Thing), typeof(Pawn), typeof(string).MakeByRefType(), typeof(bool) });
        }
        public static void Postfix(ref bool __result, Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
        {
            if (__result && pawn.IsMutant && pawn.health.hediffSet.hediffs.Any(x => x.def.HasComp(typeof(CompMindWake)) && pawn.health.hediffSet.hediffs.Any(c => c.def == HediffDefOf.GhoulBarbs || c.def == HediffDefOf.GhoulPlating)) && thing.def.apparel.bodyPartGroups.Any(c => c == BodyPartGroupDefOf.Torso || c == BodyPartGroupDefOf.Legs))
            {
                __result = false;
                cantReason = "GhoulCantWearDueToImplants".Translate();
            }
            else
            {
                cantReason = null;
            }
        }
    }
    #endregion
}
