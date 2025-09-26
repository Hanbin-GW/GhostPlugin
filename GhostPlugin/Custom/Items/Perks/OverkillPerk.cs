using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Abilities.Active;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Perks
{
    [CustomItem(ItemType.Coin)]
    public class OverkillPerk : CustomItem
    {
        public override uint Id { get; set; } = 66;
        public override string Name { get; set; } = "OverKill Perk";
        public override string Description { get; set; } = "동전을 돌릴시 오버킬 능력을 얻을수 있습니다!\n오버킬: 오버킬 능력 단축키 눌를시 랜덤 총기지금 (설정들어가서 Server-Specific 에서 확인)";
        public override float Weight { get; set; } = 1f;
        public override ItemType Type { get; set; } = ItemType.Coin;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 4,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside173Connector,
                    Chance = 80
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
                }
            }
        };        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                Plugin.Instance.PerkEventHandlers.GrantAbility(ev.Player, new Overkill());
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