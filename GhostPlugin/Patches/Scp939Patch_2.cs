using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Exiled.API.Features;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp939;
using UnityEngine;

namespace GhostPlugin.Patches
{
    [HarmonyPatch(typeof(Scp939ClawAbility), nameof(Scp939ClawAbility.ServerProcessCmd))]
    public class Scp939Patch_2
    {
        
        /*static MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(Scp939ClawAbility), "BaseCooldown");
        }
        
        static void Prefix(Scp939ClawAbility __instance)
        {
            typeof(Scp939ClawAbility).GetProperty("BaseCooldown")?
                .SetValue(__instance, 0.3f);
            Log.Info("BaseCooldown 호출됨. 기존 값: " + __instance.Cooldown);
            Log.Info("BaseCooldown이 공격 전에 수정되었습니다.");
        }*/
        
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                // BaseCooldown 값을 변경
                if (instruction.opcode == OpCodes.Ldfld && instruction.operand.ToString().Contains("BaseCooldown"))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0.3f); // 0.3으로 변경
                    continue;
                }
                yield return instruction;
            }
        }
    }
}