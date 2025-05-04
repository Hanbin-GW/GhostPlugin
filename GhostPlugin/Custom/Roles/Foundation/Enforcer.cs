using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class Enforcer : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 18;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "Enforcer (집행자)";
        public override string Description { get; set; } = "EOD 방탄복과 특수 자동샷건 (리버터)을 보유하고 있습니다! 적들을 분쇠하십시요!";
        public override string CustomInfo { get; set; } = "Enforcer";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 70;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),//Keycard
            11.ToString(), //EOD Padding
            51.ToString(), //Riveter
            28.ToString(), //Armor Plate Kit
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