using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeHE)]
    public class ImpactGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 29;
        public override string Name { get; set; } = "<color=#f56342>Impact Grenade</color>";
        public override string Description { get; set; } = "충격시 즉시 폭발하는 수류탄입니다!";
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
    }
}