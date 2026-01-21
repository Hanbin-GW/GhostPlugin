using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class AdvancedMtf : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 22;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "<color=#3bffe2>Advanced Ops</color>";
        public override string Description { get; set; } = "고급 무기들을 가지고 있습니다";
        public override string CustomInfo { get; set; } = "Advanced MTF";
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 80;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.ArmorCombat.ToString(),
            46.ToString(),
            ItemType.SCP500.ToString(),
            31.ToString(),
            9.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo12Gauge , 24},
            { AmmoType.Nato556, 120 },
            { AmmoType.Nato9 ,150},
        };
    }
}