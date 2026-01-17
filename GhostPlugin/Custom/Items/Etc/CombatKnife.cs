using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using GhostPlugin.API;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    public class CombatKnife : CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 73;
        public override string Name { get; set; } = "컴뱃 나이프";

        public override string Description { get; set; } = "칼날이 매우 치명적인 나이프 입니다.";
        public override float Weight { get; set; } = 3f;
        private readonly Dictionary<Player, float> _lastSwingTime = new();
        [Description("Cooldown time in seconds between swings.")]
        public float SwingCooldown { get; set; } = 1.5f;
        [Description("Use {time} to fetch the remaining cooldown time.")]
        public string CooldownMessage { get; set; } = "컴뱃 나이프가 {time} 초동안 쿨다운중입니다.";
        public float MessageDuration { get; set; } = 5f;
        public bool UseHints { get; set; } = true;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            DynamicSpawnPoints = new()
            {
                new()
                {
                    Chance = 10,
                    Location = SpawnLocationType.Inside049Armory
                },
            },
            LockerSpawnPoints = new()
            {
                new()
                {
                    Chance = 10,
                    Type = LockerType.LargeGun
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.SCP1509;
        public bool HasCustomItemGlow { get; set; } = true;

        private void OnTriggeringAttack(TriggeringAttackEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            float currentTime = Time.time;
            
            if (_lastSwingTime.TryGetValue(ev.Player, out float lastTime))
            {
                if (currentTime - lastTime < SwingCooldown)
                {
                    var cooldownTimeRemaining = SwingCooldown - currentTime - lastTime;
                    ev.IsAllowed = false;
                    if (!string.IsNullOrWhiteSpace(CooldownMessage))
                        if (UseHints)
                            ev.Player.ShowHint(CooldownMessage.Replace("{percent}", cooldownTimeRemaining.ToString()), MessageDuration);
                        else
                            ev.Player.Broadcast((ushort)MessageDuration, CooldownMessage.Replace("{percent}", cooldownTimeRemaining.ToString()));
                    
                    Log.Debug($"VVUP Custom Items, Knife: Attack by {ev.Player} blocked due to cooldown");
                    return;
                }
            }
    
            _lastSwingTime[ev.Player] = currentTime;
            Log.Debug($"VVUP Custom Items, Knife: Allowed swing by {ev.Player}");
            ev.IsAllowed = true;
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker.CurrentItem))
            {
                ev.Amount = 95;
            }
        }
        private void On1509Resurrecting(Exiled.Events.EventArgs.Scp1509.ResurrectingEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            Log.Debug($"GhostPlugin Custom Items, Knife: Prevented resurrection of {ev.Target} by {ev.Player}");
            ev.IsAllowed = false;
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp1509.Resurrecting += On1509Resurrecting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack += OnTriggeringAttack;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp1509.Resurrecting -= On1509Resurrecting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack -= OnTriggeringAttack;
            base.UnsubscribeEvents();
        }

        public Color CustomItemGlowColor { get; set; } = new Color32(255, 225,0, 127);
        public float GlowRange { get; set; }
        public float GlowIntensity { get; set; }
    }
}