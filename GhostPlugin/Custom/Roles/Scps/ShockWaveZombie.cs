using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp0492)]
    public class ShockWaveZombie : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 44;
        public override int MaxHealth { get; set; } = 650;
        public override string Name { get; set; } = "쇼크왜이브 좀비";
        public override string Description { get; set; } = "능력을 사용하여 충격파를 생성하십시요!";
        public override string CustomInfo { get; set; } = "Shockwave Zombie";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public StartTeam StartTeam { get; set; } =StartTeam.Scp | StartTeam.Revived;
        public int Chance { get; set; } = 90;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new Shockwave()
        };

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        protected override void RoleAdded(Player player)
        {
            player.DisableEffect<Slowness>();
            base.RoleAdded(player);
        }
    }
}