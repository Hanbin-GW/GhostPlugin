using System.Collections.Generic;
using Exiled.CustomRoles.API.Features;
using Exiled.API.Features.Attributes;
using PlayerRoles;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Passive;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp0492)]
    public class ExplosiveZombie : CustomRole, ICustomRole
    {
        public int Chance { get; set; } = 80;
        public override uint Id { get; set; } = 42;
        public override int MaxHealth { get; set; } = 500;
        public override string Name { get; set; } = "<color=#FF0000>Ballistic SCP-049-2</color>";
        public override string Description { get; set; } = "죽을시 자폭하는 좀비입니다.";
        public override string CustomInfo { get; set; } = "Ballistic SCP-049-2";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new Martyrdom
            {
                Name = "Explode on Death [Passive]",
                Description = "Causes you to explode on death",
                ExplosiveFuse = 0.5f,
            },
            new FriendlyFireRemover
            {
                Name = "Friendly Fire Remover [Passive]",
                Description = "Removes friendly fire to your team",
                TimeBeforeRemovingAbility = 1f,
            },
        };
    }
}