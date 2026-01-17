using System.Collections.Generic;
using System.IO;
using CustomPlayerEffects;
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
    public class EnergyBlade : CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 74;
        public override string Name { get; set; } = "Energy Blade";
        public override string Description { get; set; } = "엄청난 파괴력을 가진 에너지 블래이드입니다.";
        public override float Weight { get; set; } = 18f;
        public override SpawnProperties SpawnProperties { get; set; }
        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(255, 15, 255, 121);
        public float GlowRange { get; set; } = 3.5f;
        public float GlowIntensity { get; set; } = 50f;
        public float SwingCooldown { get; set; } = 1.05f;
        private readonly Dictionary<Player, float> _lastSwingTime = new();
        public string CooldownMessage { get; set; } = "에너지 블래이드가 {time} 초동안 쿨다운중입니다.";
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

            // 폭발 데미지는 무시(재귀 방지)
            if (ev.DamageHandler is ExplosionDamageHandler)
                return;

            // ✅ 한 번 공격당 폭발 1회 제한 (공격자+피격자 페어)
            var key = (ev.Attacker.Id, ev.Player.Id);
            float now = Time.time;

            if (_lastExplodeTime.TryGetValue(key, out float last) && now - last < ExplodeCooldown)
                return;

            _lastExplodeTime[key] = now;

            // --- 이하 기존 로직 ---
            if (ev.Player.IsScp)
            {
                ev.Amount = 150;
                ev.Attacker.IsGodModeEnabled = true;

                Timing.CallDelayed(0f, () =>
                {
                    if (ev.Attacker != null && ev.Player != null)
                    {
                        ev.Player.Explode(ProjectileType.Flashbang, attacker: ev.Attacker);
                        Timing.CallDelayed(1.5f, () => ev.Player.DisableEffect<Flashed>());
                    }
                    // ev.Player.Explode(ProjectileType.FragGrenade, attacker: ev.Attacker);
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
                    {
                        ev.Player.Explode(ProjectileType.Flashbang, attacker: ev.Attacker);
                        Timing.CallDelayed(1.5f, () => ev.Player.DisableEffect<Flashed>());
                    }
                });

                Timing.CallDelayed(0.5f, () => { if (ev.Attacker != null) ev.Attacker.IsGodModeEnabled = false; });
            }
        }


        private void OnDropping(DroppingItemEventArgs ev)
        {
            if (ev.Item != null && Check(ev.Item))
            {
                OrbitPrimitiveMethods.StopOrbit(ev.Player);
                OrbitPrimitiveMethods.StartTrailOverOrbit(ev.Player);
            }
        }

        protected override void OnChanging(ChangingItemEventArgs ev)
        {
            // 바뀐 뒤에 들게 될 아이템이 타겟이면 켜고, 아니면 끔
            if (ev.Item != null && Check(ev.Item))
            {
                Color color = new Color32(255, 15, 255, 121);
                Color glowColor = new Color(color.r * 50f, color.g * 50f, color.b * 50f, color.a);
                // OrbitPrimitiveMethods.StartOrbit(
                //     ev.Player,
                //     primitiveType: PrimitiveType.Cube,
                //     count: 24,
                //     color: glowColor,
                //     pattern: OrbitPrimitiveMethods.PatternMode.Orbit,
                //     motion: OrbitPrimitiveMethods.MotionMode.Chaos,
                //     speed: 1.5f,
                //     ringRadius: 0.85f,
                //     ringThickness: 0.03f
                // );
                OrbitPrimitiveMethods.StartOrbit(
                    ev.Player,
                    count: 12,
                    primitiveType: PrimitiveType.Cube,
                    color: glowColor,
                    pattern: OrbitPrimitiveMethods.PatternMode.Orbit,
                    anchorMode: OrbitPrimitiveMethods.AnchorMode.HandRight,
                    motion: OrbitPrimitiveMethods.MotionMode.Chaos,
                    // chaosRadius: 0.35f,
                    chaosRadius: 0.45f,
                    chaosSpeed: 1.5f,
                    chaosHeight: 0.17f
                );
                Color colorTrail = new Color32(255, 30, 255, 121);
                Color glowColorTrail = new Color(colorTrail.r * 50f, colorTrail.g * 50f, colorTrail.b * 50f, colorTrail.a);
                OrbitPrimitiveMethods.StartTrailOverOrbit(ev.Player, color: glowColorTrail);

            }
            else
            {
                OrbitPrimitiveMethods.StartTrailOverOrbit(ev.Player);
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