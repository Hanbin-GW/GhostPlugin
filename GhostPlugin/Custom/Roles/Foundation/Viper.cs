using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.FacilityGuard)]
    public class Viper : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 9;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Biochemist Guard";
        public override string Description { get; set; } = "He's a security guard armed with biochemical weapons.";
        public override string CustomInfo { get; set; } = "Biochemist Guard";
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public StartTeam StartTeam { get; set; } = StartTeam.Guard;
        public int Chance { get; set; } = 80;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            8.ToString(),
            40.ToString(),
            40.ToString(),
            ItemType.ArmorLight.ToString(),
            ItemType.KeycardGuard.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Radio.ToString(),
        };

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 60 },
        };
    }
}