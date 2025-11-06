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
        public override string Description { get; set; } = "Enforcer는 전투 전문 직업으로, 강력한 근접 공격과 방어 능력을 갖추고 있습니다. 이들은 적을 물리치고 팀원들을 보호하는 데 특화되어 있습니다.\n\nEOD 방탄복과 특수 자동샷건 (리버터)을 보유하고 있습니다! 적들을 분쇠하십시요!<size=18>\n------------보유중인 능력------------\n• 처치시 부스트 (패시브 & SSSS 설정 불가능)\n-------------------------------------</size>";
        public override string CustomInfo { get; set; } = "Enforcer";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 40;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new BoostOnKill()
            {
                Name = "Boost On Kill",
                Description = "적을 처치할 때마다 이동속도가 증가합니다.",
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