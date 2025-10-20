using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using Exiled.API.Features.Attributes;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosRepressor)]
    public class Commando : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 28;
        public override int MaxHealth { get; set; } = 130;
        public override string Name { get; set; } = "코만도";
        public override string Description { get; set; } = "기관총 과 바주카포로 적들을 파괴하십시오!";
        public override string CustomInfo { get; set; } = "Commando";
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 70;

        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            72.ToString(), // FR MG 03
            71.ToString(), // HE-1
            ItemType.ArmorCombat.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Adrenaline.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 200 },
        };
    }
}