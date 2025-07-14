using Exiled.API.Features;

namespace GhostPlugin.Methods.CustomRole
{
    public class RoleMethods
    {
        public bool HasCustomRole(Player player, uint roleId)
        {
            foreach (var role in CustomRole.)
            {
                if (role.Id == roleId && role.Check(player))
                    return true;
            }

            return false;
        }
    }
}