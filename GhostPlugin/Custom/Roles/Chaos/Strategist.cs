using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosMarauder)]
    public class Strategist : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 19;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "Strategist";
        public override string Description { get; set; } = "The leader of a chaotic rebellion that has led many battlefields to victory with various strategies in combat!";
        public override string CustomInfo { get; set; } = "Strategist";
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosMarauder;
        public int Chance { get; set; } = 90;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            50.ToString(),
            ItemType.ArmorCombat.ToString(),
            19.ToString(),
            9.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Radio.ToString()
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new EnhancedGoggleVision()
            {
                Name = "Enhanced Goggle Vision",
                Description = "You can temporarily see the enemy over the wall!"
            },
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762 ,150},
        };
    }
}