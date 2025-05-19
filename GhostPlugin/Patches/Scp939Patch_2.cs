using HarmonyLib;
using PlayerRoles.PlayableScps.Scp939;

namespace GhostPlugin.Patches
{
    [HarmonyPatch(typeof(Scp939ClawAbility), "BaseCooldown", MethodType.Getter)]
    public class Scp939Patch_2
    {
        public static void Postfix(ref float __result)
        {
            __result = 0.5f;
        }
    }
}