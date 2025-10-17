using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Passive;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfCaptain)]
    public class Director : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 27;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "MTF Overseer";
        public override string Description { get; set; } = "You are overseer in TaskForce";
        public override string CustomInfo { get; set; } = "Overseer";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfCaptain;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 50;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new HealOnKill()
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFCaptain.ToString(),
            ItemType.ArmorHeavy.ToString(),
            ItemType.GunFRMG0.ToString(),
            ItemType.AntiSCP207.ToString(),
            ItemType.Adrenaline.ToString(),
            10.ToString(),
            34.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556 ,180},
            { AmmoType.Nato9 ,30},
        };
    }
}