using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerStatsSystem;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class Ballista : CustomWeapon
    {
        public override byte ClipSize { get; set; } = 10;
        public override ItemType Type { get; set; } = ItemType.ParticleDisruptor;
        public override uint Id { get; set; } = 53;
        public override string Name { get; set; } = "Ballista";
        public override string Description { get; set; } = "It's a laser canon that penetrates the wall!";
        public override float Weight { get; set; } = 10;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 20,
                    Location = SpawnLocationType.InsideHidChamber,
                },
                new ()
                {
                    Chance = 20,
                    Location = SpawnLocationType.Inside079Armory,
                },
            },
        };
        public override float Damage { get; set; }
        public List<float> LaserColorRed { get; set; } = new List<float>()
        {
            0.86f, 
            1, 
            0,
            0.55f,
            0.97f,
        };
        public List<float> LaserColorGreen { get; set; } = new List<float>()
        {
            0.08f,
            0.27f,
            0.84f,
            0.65f,
            0.5f,
            0.36f,
            0,
            0.97f,
        };
        public List<float> LaserColorBlue { get; set; } = new List<float>()
        {
            0.24f,
            0,
            0.31f,
            1,
            0.96f,
        };


        public float LaserVisibleTime { get; set; } = 0.5f;
        public Vector3 LaserScale { get; set; } = new Vector3(0.05f, 0.05f, 0.05f);
        
        protected override void SubscribeEvents()
        {
            Player.Shot += OnShot;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Player.Shot -= OnShot;
            base.UnsubscribeEvents();
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem)) return;
            ev.CanHurt = false;

            var laserColor = new Color(0f, 1f, 1f, 0.1f) * 50;
            var origin = ev.Player.CameraTransform.position + ev.Player.CameraTransform.forward * 0.3f;
            var direction = ev.Player.CameraTransform.forward;
            float distance = 200f;

            // 시각 효과 (무조건 직진으로 200 거리)
            var laserPos = origin + direction * (distance * 0.5f);
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
            
            var scale = new Vector3(LaserScale.x, distance * 0.5f, LaserScale.z);

            var laser = Primitive.Create(
                PrimitiveType.Cylinder,
                PrimitiveFlags.Visible,
                laserPos,
                rotation.eulerAngles,
                scale,
                true,
                laserColor
            );
            var damagedPlayers = new HashSet<Exiled.API.Features.Player>();
            // 대미지: 레이저에 맞은 사람 탐지
            var hits = Physics.RaycastAll(origin, direction, distance);
            foreach (var hit in hits)
            {
                if (ev?.Target == null)
                {
                    Log.Warn("[OnDying] ev or ev.Player is null");
                    return;
                }

                Log.Debug($"[OnDying] Player {ev.Target.Nickname} died.");

                if (ev.Player != null)
                    Log.Debug($"Killed by: {ev.Player.Nickname}");
                var hub = hit.collider.GetComponentInParent<ReferenceHub>();
                var target = Exiled.API.Features.Player.Get(hub);
                if(target == null || target == ev.Player || damagedPlayers.Contains(target)) 
                    continue;

                if (!target.IsAlive || target.LeadingTeam == ev.Player.LeadingTeam)
                    continue;

                damagedPlayers.Add(target);
                ev.Player.ShowHitMarker(1.5f);

                target.Hurt(new CustomReasonDamageHandler( "Laser penetration", 100));

            }

            Timing.CallDelayed(LaserVisibleTime, laser.Destroy);
        }
    }
}