using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Passive;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class Enforcer : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 18;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "<color=red>Enforcer (집행자)</color>";
        public override string Description { get; set; } = "Enforcer is a profession specializing in combat, with strong close attacks and defensive capabilities. They specialize in defeating enemies and protecting team members.";
        public override string CustomInfo { get; set; } = "Enforcer";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 30;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new BoostOnKill()
            {
                Name = "Boost On Kill",
                Description = "when you kills the enemy, your will get a movement boost.",
            }
        };
        
        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),//Keycard
            11.ToString(), //EOD Padding
            51.ToString(), //Riveter
            28.ToString(), //Armor Plate Kit
            30.ToString(),
            34.ToString(), //C4
            ItemType.Radio.ToString(), //Radio
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 60 },
            { AmmoType.Nato9 , 30},
        };
    }
}