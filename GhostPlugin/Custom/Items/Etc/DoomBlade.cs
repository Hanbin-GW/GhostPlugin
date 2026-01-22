using System.Collections.Generic;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using GhostPlugin.API;
using GhostPlugin.Methods.Music;
using GhostPlugin.Methods.ParticlePrimitives;
using MEC;
using PlayerStatsSystem;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    public class DoomBlade : CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 75;
        public override string Name { get; set; } = "Doom Blade";
        public override string Description { get; set; } = "Blades processed with devastating energy.\n(Critical Damage to SCP + 30sec cooldown)";
        public override float Weight { get; set; } = 18f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideGateA,
                    Chance = 50
                }
            }
        };
        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(255, 165, 0, 121);
        public float GlowRange { get; set; } = 3.5f;
        public float GlowIntensity { get; set; } = 50f;
        public float SwingCooldown { get; set; } = 30f;
        private readonly Dictionary<Player, float> _lastSwingTime = new();
        public string CooldownMessage { get; set; } = "DoomBlade is cooldown for {time} seconds.";
        public float MessageDuration { get; set; } = 5f;
        public bool UseHints { get; set; } = true;
        public override ItemType Type { get; set; } = ItemType.SCP1509;
        private readonly Dictionary<(int attackerId, int victimId), float> _lastExplodeTime = new();
        private const float ExplodeCooldown = 0.35f; // 0.25~0.5 사이 추천

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
                    string fileName = "Titan tvman sword sound 2.ogg";
                    string path = Path.Combine(Plugin.Instance.EffectDirectory, fileName);
                    float duration = API.Audio.AudioUtils.GetOggDurationInSeconds(path);
                    MusicMethods.PlaySoundEffect(fileName,ev.Player,duration,7.5f);

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
            if (ev.Attacker == null || ev.Attacker.CurrentItem == null || ev.Player == null)
                return;

            if (!Check(ev.Attacker.CurrentItem))
                return;

            if (ev.DamageHandler is ExplosionDamageHandler)
                return;

            var key = (ev.Attacker.Id, ev.Player.Id);
            float now = Time.time;

            if (_lastExplodeTime.TryGetValue(key, out float last) && now - last < ExplodeCooldown)
                return;

            _lastExplodeTime[key] = now;

            // --- 이하 기존 로직 ---
            if (ev.Player.IsScp)
            {
                ev.Amount = 100;
                ev.Attacker.IsGodModeEnabled = true;

                Timing.CallDelayed(0f, () =>
                {
                    if (ev.Attacker != null && ev.Player != null)
                        ev.Player.Explode(ProjectileType.FragGrenade, attacker: ev.Attacker);
                });

                Timing.CallDelayed(0.5f, () => { if (ev.Attacker != null) ev.Attacker.IsGodModeEnabled = false; });
            }
            else if (ev.Player.IsHuman)
            {
                ev.Amount = 100;
                ev.Attacker.IsGodModeEnabled = true;

                Timing.CallDelayed(0f, () =>
                {
                    if (ev.Attacker != null && ev.Player != null)
                        ev.Player.Explode(ProjectileType.Flashbang, attacker: ev.Attacker);
                });

                Timing.CallDelayed(0.5f, () => { if (ev.Attacker != null) ev.Attacker.IsGodModeEnabled = false; });
            }
        }


        private void OnDropping(DroppingItemEventArgs ev)
        {
            if (ev.Item != null && Check(ev.Item))
                OrbitPrimitiveMethods.StopOrbit(ev.Player);
        }

        protected override void OnChanging(ChangingItemEventArgs ev)
        {
            if (ev.Item != null && Check(ev.Item))
            {
                Color color = new Color32(255, 165, 0, 121);;
                Color glowColor = new Color(color.r * 50f, color.g * 50f, color.b * 50f, color.a);
                OrbitPrimitiveMethods.StartOrbit(
                    ev.Player,
                    count: 12,
                    color: glowColor,
                    anchorMode: OrbitPrimitiveMethods.AnchorMode.HandRight,
                    motion: OrbitPrimitiveMethods.MotionMode.Chaos,
                    // chaosRadius: 0.35f,
                    chaosRadius: 0.45f,
                    chaosSpeed: 2f,
                    chaosHeight: 0.17f
                );

            }
            else
            {
                OrbitPrimitiveMethods.StopOrbit(ev.Player);
            }

            base.OnChanging(ev);
        }

        private void On1509Resurrecting(ResurrectingEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            ev.IsAllowed = false;
        }
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp1509.Resurrecting += On1509Resurrecting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack += OnTriggeringAttack;
            Exiled.Events.Handlers.Player.DroppingItem += OnDropping;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp1509.Resurrecting -= On1509Resurrecting;
            Exiled.Events.Handlers.Scp1509.TriggeringAttack -= OnTriggeringAttack;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDropping;
            base.UnsubscribeEvents();
        }
    }
}