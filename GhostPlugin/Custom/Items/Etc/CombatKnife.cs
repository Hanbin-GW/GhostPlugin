using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using GhostPlugin.API;
using InventorySystem.Items.Scp1509;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    public class CombatKnife : CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 73;
        public override string Name { get; set; } = "컴뱃 나이프";

        public override string Description { get; set; } = "칼날이 매우 치명적인 나이프 입니다.";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside106Primary,
                    Chance = 100,
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.SCP1509;
        public bool HasCustomItemGlow { get; set; } = true;

        private void OnTriggeringAttack(TriggeringAttackEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                var item = (Scp1509Item)ev.Scp1509.Base;
                item._meleeDelay = 3f;
                item._meleeCooldown = 3f;

                // 3. 실제 쿨다운 타이머 리셋
                item._clientAttackCooldown.Trigger(3);
                item._clientDelayCooldown.Trigger(3);
                item._serverAttackCooldown.Trigger(3);
                
                ev.Scp1509.RespawnEligibility.enabled = false;
                //ev.Scp1509.Base._meleeCooldown = 3f;
                ev.Scp1509.MeleeCooldown = 3f;
            }
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker.CurrentItem))
            {
                ev.Amount = 95;
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack += OnTriggeringAttack;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack -= OnTriggeringAttack;
            base.UnsubscribeEvents();
        }

        public Color CustomItemGlowColor { get; set; } = new Color32(255, 225,0, 127);

    }
}