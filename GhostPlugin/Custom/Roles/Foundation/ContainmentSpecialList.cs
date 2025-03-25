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
    public class ContainmentSpecialList : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 12;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "<color=#0096FF>Containment SpecialList</color>";
        public override string Description { get; set; } = "SCP 격리 전문 요원입니다.";
        public override string CustomInfo { get; set; } = "Containment SpecialList";
        public StartTeam StartTeam { get; set; } = StartTeam.Guard;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public int Chance { get; set; } = 30;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>()
            {
                new RoleSpawnPoint()
                {
                    Role = RoleTypeId.FacilityGuard,
                    Chance = 100
                }
            }
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardContainmentEngineer.ToString(),
            ItemType.ArmorCombat.ToString(),
            12.ToString(),
            30.ToString(),
            13.ToString(),
            43.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762 ,100},
        };

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }
    }
}