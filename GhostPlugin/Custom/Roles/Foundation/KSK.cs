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
    public class Ksk : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 30;
        public override int MaxHealth { get; set; } = 130;
        public override string Name { get; set; } = "<color=#0096FF>KSK Hunter Unit</color>";
        public override string Description { get; set; } = "당신은 SCP 정예부대 이자 SCP682 격리 요원입니다..!\n682를 격리해 제단을 지키십시요..!";
        public override string CustomInfo { get; set; } = "KSK Hunter Unit";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>()
            {
                new RoleSpawnPoint()
                {
                    Role = RoleTypeId.NtfPrivate,
                    Chance = 100,
                }
            }
        };
        public StartTeam StartTeam { get; set; } = StartTeam.Other;
        public int Chance { get; set; } = 0;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.ArmorHeavy.ToString(),
            ItemType.KeycardMTFCaptain.ToString(),
            ItemType.MicroHID.ToString(),
            11.ToString(),
            ItemType.GrenadeHE.ToString(),
            ItemType.GrenadeHE.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 200 },
            { AmmoType.Nato556, 200 },
            { AmmoType.Nato762, 200 },
            { AmmoType.Ammo44Cal ,2},
        };
    }
}