using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosRepressor)]
    public class Ninja : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 7;
        public override int MaxHealth { get; set; } = 110;
        public override string Name { get; set; } = "<color=#58b09d>Ninja</color>";
        public override string Description { get; set; } = "조용히 움직이는 닌자입니다. 연막탄과 특수 리볼버가 제공되었습니다.";
        public override string CustomInfo { get; set; } = "Ninja";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 30;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.Painkillers.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.GunShotgun.ToString(),
            44.ToString(),
            20.ToString(),
            20.ToString(),
            13.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo12Gauge, 32 },
            { AmmoType.Ammo44Cal, 15 }
        };

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect<SilentWalk>();
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect<SilentWalk>();
            base.RoleRemoved(player);
        }
    }
}