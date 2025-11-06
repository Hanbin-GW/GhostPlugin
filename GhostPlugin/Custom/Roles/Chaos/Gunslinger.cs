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
    [CustomRole(RoleTypeId.ChaosConscript)]
    public class Gunslinger : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 15;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Gunslinger";
        public override string Description { get; set; } = "총기를 아주 잘 조작하는 요원입니다.\n<size=18>--------------보유중인 능력--------------\n• 오버킬(랜덤 무기 제공 | SSSS 에서 키바인드 설정)\n• 집중(장전속도 & 반동 감소 | SSSS 에서 키바인드 설정)\n-------------------------------------</size>";
        public override string CustomInfo { get; set; } = "Gunslinger";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosConscript;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 90;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new Overkill()
            {
                Name = "오버킬",
                Description = "능력 활성화시, 랜덤으로 총기를 획득합니다! (쿨다운 45초)",
            },
            new Focus()
            {
                Name = "집중",
                Description = "사용시 극단의 SCP1853 이 적용됩니다!",
            }
        };
        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.ArmorCombat.ToString(),
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.Painkillers.ToString(),
            13.ToString(),
            44.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 120 },
            { AmmoType.Nato556, 120 },
            { AmmoType.Nato9, 150 },
            { AmmoType.Ammo44Cal, 20},
        };
    }
}