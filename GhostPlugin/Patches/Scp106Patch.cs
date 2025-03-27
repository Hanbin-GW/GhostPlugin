using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp106;

namespace GhostPlugin.Patches
{
    [HarmonyPatch(typeof(Scp106Attack))]
    public class Scp106Patch
    {
        public static bool ShouldTp(bool oldState)
        {
            return true;
        }
        
        static MethodBase TargetMethod()
        {
            // Use AccessTools to get the private method
            return AccessTools.Method(typeof(Scp106Attack), "ServerShoot");
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var index = 48;

            Collection<CodeInstruction> collection = new()
            {
                new(OpCodes.Call, Method(typeof(Scp106Patch), nameof(ShouldTp), new [] { typeof(bool) }))
            };

            newInstructions.InsertRange(index, collection);

            foreach (CodeInstruction instruction in newInstructions)
            {
                yield return instruction;
            }

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
