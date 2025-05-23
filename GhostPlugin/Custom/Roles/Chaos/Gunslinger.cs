using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;

namespace GhostPlugin.Custom.Roles.Chaos
{
    public class Gunslinger : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 15;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Gunslinger";
        public override string Description { get; set; } = "오버킬 조끼를 갖고있는 요원입니다.";
        public override string CustomInfo { get; set; } = "Gunslinger";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 90;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new Overkill(),
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            45.ToString(),
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.Painkillers.ToString(),
            ItemType.GunAK.ToString(),
            44.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 120 },
            { AmmoType.Nato556, 120 },
            { AmmoType.Nato9, 150 },
        };
    }
}