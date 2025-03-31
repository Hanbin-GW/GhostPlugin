using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Coin)]
    public class EscapeDevice : CustomItem
    {
        public override uint Id { get; set; } = 5;
        public override string Name { get; set; } = "Escape Device";
        public override string Description { get; set; } = "비상시에만 사용하세요";
        public override float Weight { get; set; } = 0.5f;
        public override ItemType Type { get; set; } = ItemType.Coin;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Zone = ZoneType.HeavyContainment,
                    Chance = 30,
                    Type = LockerType.Scp244Pedestal
                }
            }
        };
    }
}