using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Custom.Abilities.Passive;
using GhostPlugin.EventHandlers;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Perks
{
    [CustomItem(ItemType.Coin)]
    public class FocusPerk : CustomItem
    {
        public override uint Id { get; set; } = 62;
        public override string Name { get; set; } = "Focus Perk";
        public override string Description { get; set; } = "동전을 돌릴시 집중 능력을 얻을수 있습니다!\n집중: 무기 재장선 속도와, 반동, 조준시간이 감소합니다. (설정 -> Server-Specific 에서 단축키 설정 가능)";
        public override float Weight { get; set; } = 1f;
        public override ItemType Type { get; set; } = ItemType.Coin;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
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
                    Type = LockerType.LargeGun,
                    Chance = 100,
                    UseChamber = true,
                    Offset = new Vector3(1,1.25f,2),
                }
            }
        };
        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                Plugin.Instance.PerkEventHandlers.GrantAbility(ev.Player, new Focus());
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