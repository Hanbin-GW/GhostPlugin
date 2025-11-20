using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    public class ReconJuggernaut : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 29;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "정찰 저거넛";
        public override string Description { get; set; } = "휴대용 방패와 샷건을 보유한 저거너트 입니다. 적을 재압하십시요.";
        public override string CustomInfo { get; set; } = "Recon Juggernaut";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSpecialist;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 55;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            70.ToString(),
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.GunShotgun.ToString(),
            54.ToString(),
            ItemType.Adrenaline.ToString(),
            ItemType.Radio.ToString()
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo12Gauge ,36},
        };
    }
}