using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorCombat)]
    public class ActiveArmor : CustomArmor
    {
        private readonly Dictionary<Player, CoroutineHandle> activeArmor = new ();
        public override uint Id { get; set; } = 69;
        public override string Name { get; set; } = "반응형 장갑판 주머니";
        public override string Description { get; set; } = "적 처치 / 부상시 AHP 가 적용됩니다.";
        public override float Weight { get; set; } = 6f;
        public override ItemType Type { get; set; } = ItemType.ArmorCombat;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideLczArmory,
                    Chance = 50,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideGr18,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside106Primary,
                    Chance = 15,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideGr18Glass,
                    Chance = 10
                }
            }
        };
        public float RepairAmount { get; set; } = 25f;
        public float RepairOverTimeDuration { get; set; } = 10f;
        public float RepairOverTimeTickFrequency { get; set; } = 1.0f;
        public bool HealOverTime { get; set; } = true;
        public bool DamageInterruptsHot { get; set; } = true;
        
        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            player.AddAhp(25);
            base.OnAcquired(player, item, displayMessage);
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.Player.ArtificialHealth = 0;
            }
            base.OnDroppingItem(ev);
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Attacker.CurrentArmor))
            {
                if (HealOverTime)
                    activeArmor[ev.Attacker] = Timing.RunCoroutine(RepairOverTime(ev.Attacker));
                ev.Attacker.AddAhp(RepairAmount);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor))
            {
                if (DamageInterruptsHot && activeArmor.ContainsKey(ev.Player))
                    Timing.KillCoroutines(activeArmor[ev.Player]);
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }
        
        private IEnumerator<float> RepairOverTime(Player player)
        {
            float tickAmount = RepairAmount / RepairOverTimeDuration;
            int tickCount = Mathf.FloorToInt(RepairOverTimeDuration / RepairOverTimeTickFrequency);

            for (int i = 0; i < tickCount; i++)
            {
                player.AddAhp(tickAmount);
                yield return Timing.WaitForSeconds(RepairOverTimeTickFrequency);
            }
        }
    }
}