using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Custom.Abilities.Passive;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfSergeant)]
    public class Elite : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 4;
        public override int MaxHealth { get; set; } = 110;
        public override string Name { get; set; } = "<color=#0096FF>Elite Agent</color>";
        public override string Description { get; set; } = "제단의 엘리트 요원";
        public override string CustomInfo { get; set; } = "Elite Agent";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public bool DebugMode = false;
        public int Chance { get; set; } = 40;
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new HealOnKill()
            {
                Name = "HealOnKill",
                Description = "Heals the player when they kill someone.",
            },
            new EnhancedGoggleVision()
            {
                Name = "Enhance Vision",
                Description = "Scp-1344 의 효과를 일시적으로 부여"
            }
        };
        public override List<string> Inventory { get; set; } = new()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.GunE11SR.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Radio.ToString(),
            28.ToString(),
            44.ToString(),
            10.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 120 },
            { AmmoType.Ammo44Cal, 20 }
        };
    
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>()
            {
                new()
                {
                    Role = RoleTypeId.NtfSergeant,
                    Chance = 85,
                }
            }
        };
        
        protected override void RoleAdded(Player player)
        {
            if (DebugMode == true)
            {
                player.AddAmmo(AmmoType.Nato556, 120);
                player.AddAmmo(AmmoType.Ammo44Cal,18);
            }
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            base.RoleRemoved(player);
        }

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