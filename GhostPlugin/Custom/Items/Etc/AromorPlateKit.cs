using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Medkit)]
    public class ArmorPlateKit : CustomItem
    {
        public override uint Id { get; set; } = 28;
        public override string Name { get; set; } = "<color=#32ff24>Armor Plate Kit</color>";
        public override string Description { get; set; } = "이 아이탬을 사용시,HP 회복및 <color=green>AHP</color> 를 획득합니다..";
        public override float Weight { get; set; } = 3f;
        public override ItemType Type { get; set; } = ItemType.Medkit;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Chance = 60,
                    Zone = ZoneType.HeavyContainment,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                },
                new LockerSpawnPoint()
                {
                    Chance = 100,
                    Zone = ZoneType.LightContainment,
                    Type = LockerType.Misc
                }
            }
        };

        private void OnUsed(UsedItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                var player = ev.Player;
                ev.Item.Destroy();
                player.AddAhp(amount:25,decay:0f);
            }
        }
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsed;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsed;
            base.UnsubscribeEvents();
        }
    }
}
