using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using InventorySystem.Items.Jailbird;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Jailbird)]
    public class SpikeJailbird : CustomItem
    {
        public override uint Id { get; set; } = 38;
        public override string Name { get; set; } = "<color=#0095ff>Spike Jailbird</color>";
        public override string Description { get; set; } = "Very overcharged unlimited proximity withdrawal.";
        public override float Weight { get; set; } = 6;
        public override ItemType Type { get; set; } = ItemType.Jailbird;

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Chance = 50,
                    Room = RoomType.HczNuke
                },
				new RoomSpawnPoint()
				{
					Chance = 30,
					Room = RoomType.LczAirlock
				},
                new RoomSpawnPoint()
                {
                    Chance = 70,
                    Room = RoomType.Hcz079
                }
			}
        };
        private void OnSwinging(SwingingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.Jailbird.MeleeDamage = 70;
                ev.Jailbird.WearState = JailbirdWearState.Healthy;
                //ev.Jailbird.ConcussionDuration = 5f;
                //ev.Jailbird.FlashDuration = 0f;
            }
        }

        /*private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Attacker.CurrentItem != null)
            {
                // 공격자의 아이템이 맞는지 확인하고, DamageHandler가 JailbirdDamageHandler인지 확인
                if (Check(ev.Attacker.CurrentItem) && ev.DamageHandler.Base is JailbirdDamageHandler)
                {
                    // 출혈 효과를 적용하고 플레이어에게 알림 메시지를 표시
                    ev.Player.EnableEffect<Bleeding>(duration: 10);
                    ev.Player.ShowHint("<color=red><b>출혈이 일어나고 있습니다!\n의료용 아이탬을 사용하세요!</b></color>", 10);
                }
            }
        }*/

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.Swinging += OnSwinging;
            //Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.Swinging -= OnSwinging;
            //Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }
    }
}
