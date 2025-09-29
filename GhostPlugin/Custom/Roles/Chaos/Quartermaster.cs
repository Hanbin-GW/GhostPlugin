using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosConscript)]
    public class Quartermaster : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 20;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Quartermaster (보급관)";
        public override string Description { get; set; } = "혼돈의반란 보급병입니다!\n항상뒤에서 대원들의 탄약을 보충해줍니다!\n보유중인 능력\n• Resupply (보충)ㄴ";
        public override string CustomInfo { get; set; } = "Quartermaster";
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 80;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.GunAK.ToString(),
            18.ToString(),
            18.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Adrenaline.ToString(),
            ItemType.Painkillers.ToString(),
        };
        
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new Resupply()
            {
                Name = "보충",
                Description = "수류탄과 섬광탄을 재보충해줍니다!",
            },
        };
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 90 },
        };
    }
}