using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using GhostPlugin.API;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeHE)]
    public class ImpactGrenade : CustomGrenade, ICustomItemGlow
    {
        public override uint Id { get; set; } = 29;
        public override string Name { get; set; } = "<color=#f56342>Impact Grenade</color>";
        public override string Description { get; set; } = "It's a grenade that explodes immediately in case of an impact!";
        public override float Weight { get; set; } = 3.5f;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 4,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside939Cryo,
                    Chance = 45,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside939Cryo,
                    Chance = 30,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidChamber,
                    Chance = 30,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideLczArmory
                },
                new DynamicSpawnPoint()
                {
                    Chance = 75,
                    Location = SpawnLocationType.InsideLczArmory
                }
            }
        };
        public override bool ExplodeOnCollision { get; set; } = true;
        public override float FuseTime { get; set; } = 4.5f;
        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(255, 140, 59, 200);
        public float GlowRange { get; set; } = 0.15f;
    }
}