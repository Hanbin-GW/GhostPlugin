using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using GhostPlugin.API;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    public class EnergizedBlade : CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 73;
        public override string Name { get; set; } = "Energized Blade";

        public override string Description { get; set; } =
            "This blade has a massive damage and also it can <color=green>reflect / deduct</color> the bullet damage";

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
                ev.Scp1509.MeleeCooldown = 0.25f;
            }
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker.CurrentItem))
            {
                ev.Amount = 90;
                ev.Player.EnableEffect<Burned>(duration: 2.5f);
            }

            if (Check(ev.Player.CurrentItem))
            {
                ev.Amount *= 0.2f;
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