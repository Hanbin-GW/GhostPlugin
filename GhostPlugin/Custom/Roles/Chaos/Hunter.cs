using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Methods.ParticlePrimitives;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosRepressor)]
    public class Hunter : CustomRole,ICustomRole
    {
        public override uint Id { get; set; } = 23;
        public override int MaxHealth { get; set; } = 200;
        public override string Name { get; set; } = "Hunter";
        public override string Description { get; set; } = "You are very export to assassinate the strong enemy.";
        public override string CustomInfo { get; set; } = "HUNTER";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
        public int Chance { get; set; } = 50;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new ChargeAbility(),
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Jailbird.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.GunA7.ToString(),
            44.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 150 },
            { AmmoType.Ammo44Cal, 20 },
        };
        protected override void RoleAdded(Player player)
        {

            Color color = new Color32(255, 165, 0, 121);;
            Color glowColor = new Color(color.r * 75f, color.g * 75f, color.b * 75f, color.a);
            OrbitPrimitiveMethods.StartOrbit(
                player,
                count: 10,
                color: glowColor,
                motion: OrbitPrimitiveMethods.MotionMode.Circle,
                pattern: OrbitPrimitiveMethods.PatternMode.BackRing,
                speed: 15f,
                ringRadius: 0.85f,
                ringThickness: 0.03f
            );
            OrbitPrimitiveMethods.StartTrail(player, segmentCount: 10, color: glowColor);
        }
        protected override void RoleRemoved(Player player)
        {
            OrbitPrimitiveMethods.StopOrbit(player);
            OrbitPrimitiveMethods.StopTrail(player);

        }
    }
}