using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ProjectAnomalySyndicate.HarmonyPatches
{

    public static class BloodRainTickResistanceHelper
    {
        public static float AdjustSeverityRate(Hediff_BloodRage instance, float severity)
        {
            if (instance.pawn.apparel != null)
            {
                float resistance = 0;
                foreach (Apparel thing in instance.pawn.apparel.WornApparel)
                {
                    if (thing.def.HasModExtension<BloodRainResistanceModExtension>())
                    {
                        resistance += thing.def.GetModExtension<BloodRainResistanceModExtension>().bloodRainResistance;
                    }
                }
                Log.Message(severity);
                severity *= 1f - resistance;
            }
            return severity;
        }
    }
    [HarmonyPatch]
    public static class BloodRainTickResistancePatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Hediff_BloodRage), nameof(Hediff_BloodRage.Tick));
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var skipTryInteractWith = il.DefineLabel();
            var continueTryInteractWith = il.DefineLabel();
            bool edit = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (edit)
                {
                    edit = false;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BloodRainTickResistanceHelper), nameof(BloodRainTickResistanceHelper.AdjustSeverityRate)));
                }
                if(instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == AccessTools.Field(typeof(Hediff_BloodRage), "adjustedSeverityRaisePerTick"))
                {
                    edit = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                }

                yield return instruction;
            }
        }


    }

}
