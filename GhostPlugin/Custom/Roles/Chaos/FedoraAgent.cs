using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosRepressor)]
    public class FedoraAgent : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 5;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#00ff44>FedoraAgent</color>";
        public override string Description { get; set; } = "염산무기를 가지고 있습니다!";
        public override string CustomInfo { get; set; } = "FedoraAgent";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 0;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            31.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Radio.ToString(),
            ItemType.SCP500.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 100 },
        };
    }
}