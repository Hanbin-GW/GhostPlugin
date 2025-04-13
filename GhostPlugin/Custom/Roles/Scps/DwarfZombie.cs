using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using GhostPlugin.API;
using UnityEngine;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp0492)]
    public class DwarfZombie : CustomRole, ICustomRole
    {
        public int Chance { get; set; } = 75;
        public override uint Id { get; set; } = 43;
        public override int MaxHealth { get; set; } = 200;
        public override string Name { get; set; } = "<color=#FF0000>Dwarf SCP-049-2</color>";
        public override string Description { get; set; } = "A smaller zombie";
        public override string CustomInfo { get; set; } = "Dwarf SCP-049-2";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

        protected override void RoleAdded(Player player)
        {
            player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            player.EnableEffect<MovementBoost>(intensity: 10);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.Scale = Vector3.one;
            player.DisableEffect<MovementBoost>();
            base.RoleRemoved(player);
        }
    }
}