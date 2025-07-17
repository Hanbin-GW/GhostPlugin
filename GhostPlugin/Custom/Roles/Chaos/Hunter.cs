using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosRepressor)]
    public class Hunter : CustomRole,ICustomRole
    {
        public override uint Id { get; set; } = 23;
        public override int MaxHealth { get; set; } = 200;
        public override string Name { get; set; } = "Hunter";
        public override string Description { get; set; } = "You are very export to assassinate the strong enemy.";
        public override string CustomInfo { get; set; } = "HUNTER";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
        public int Chance { get; set; } = 50;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new ChargeAbility(),
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Jailbird.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.GunA7.ToString(),
            44.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 150 },
            { AmmoType.Ammo44Cal, 20 },
        };
    }
}