using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Abilities.Passive;
using GhostPlugin.EventHandlers;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Perks
{
    [CustomItem(ItemType.Coin)]
    public class QuickfixPerk : CustomItem
    {
        public override uint Id { get; set; } = 61;
        public override string Name { get; set; } = "Quick Fix Perk";
        public override string Description { get; set; } = "동전을 돌릴시 빠른회복 능력을 얻을수 있습니다!";
        public override float Weight { get; set; } = 1f;
        public override ItemType Type { get; set; } = ItemType.Coin;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideLczArmory,
                    Chance = 100
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Type = LockerType.Medkit,
                    Chance = 100,
                    UseChamber = true,
                    Offset = new Vector3(1,2,1),
                    Zone = ZoneType.HeavyContainment & ZoneType.Entrance,
                }
            }
        };
        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                Plugin.Instance.PerkEventHandlers.GrantAbility(ev.Player, new HealOnKill());
                ev.Item.Destroy();
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            base.UnsubscribeEvents();
        }
    }
}