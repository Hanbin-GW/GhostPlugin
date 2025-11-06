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
    [CustomRole(RoleTypeId.ChaosMarauder)]
    public class Strategist : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 19;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "Strategist (전략가)";
        public override string Description { get; set; } = "전투에서 다양한 전략으로 많은 전장을 승리로 이끌고온 혼돈의 반란 대장입니다!<size=18>\n------------보유중인 능력------------\n• 향상된 비전 고글\n------------------------------------</size>";
        public override string CustomInfo { get; set; } = "Strategist";
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosMarauder;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };
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
                Name = "향상된 고글 비전",
                Description = "일시적으로 벽넘어의 적을 볼수 있습니다!"
            },
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762 ,150},
        };
    }
}