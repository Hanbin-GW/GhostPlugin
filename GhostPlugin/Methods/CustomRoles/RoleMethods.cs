
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Methods.CustomRoles
{
    public class RoleMethods
    {
        public bool HasCustomRole(Player player, string roleName)
        {
            return CustomRole.Get(roleName)?.Check(player) ?? false;
        }
    }
}