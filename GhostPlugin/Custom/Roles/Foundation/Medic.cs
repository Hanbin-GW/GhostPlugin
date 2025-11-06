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
    public class Medic : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 21;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#0096FF>MTF Medic</color>";
        public override string Description { get; set; } = "<color=#0096FF>부활키트</color>를 사용하여 아군을 회복하십시요!";
        public override string CustomInfo { get; set; } = "Medic";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 55;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.GunCrossvec.ToString(),
            ItemType.Radio.ToString(),
            ItemType.ArmorCombat.ToString(),
            "13",
            "39",
            "39",
            "20",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 120 }
        };
    }
}