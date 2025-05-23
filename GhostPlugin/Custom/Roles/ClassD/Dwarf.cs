using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Roles.ClassD
{
    public class Dwarf : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 16;
        public override int MaxHealth { get; set; } = 50;
        public override string Name { get; set; } = "Dwarf";
        public override string Description { get; set; } = "It's a very small-looking group of D-class personal.";
        public override string CustomInfo { get; set; } = "Dwarf";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public StartTeam StartTeam { get; set; } = StartTeam.ClassD;
        public int Chance { get; set; } = 70;

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(0.5f, () => player.Scale = new Vector3(0.4f, 0.4f, 0.4f));
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.CallDelayed(0.1f, () => player.Scale = Vector3.one);
            base.RoleRemoved(player);
        }
    }
}