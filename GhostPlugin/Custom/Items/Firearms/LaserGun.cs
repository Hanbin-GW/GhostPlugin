using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using PlayerStatsSystem;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;
using Random = System.Random;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class LaserGun : CustomWeapon
    {
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        public override uint Id { get; set; } = 52;
        public override string Name { get; set; } = "라스건";
        public override string Description { get; set; } = "레이저를 발사합니다!";
        public override float Weight { get; set; } = 2;
        public override byte ClipSize { get; set; } = 30;

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 20,
                    Location = SpawnLocationType.Inside106Secondary,
                },
                new ()
                {
                    Chance = 20,
                    Location = SpawnLocationType.InsideHidChamber,
                },
            },
        };
        public override float Damage { get; set; }

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.LowcapMagJHP,
            AttachmentName.MuzzleBooster,
            AttachmentName.Foregrip,
        };

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
        private void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.IsAllowed = false;
            ev.Player.ShowHint("이 아이탬은 부착물 변경이 금지되어있습니다!", 3);
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            Player.Shot += OnShot;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments -= OnChangingAttachments;
            Player.Shot -= OnShot;
            base.UnsubscribeEvents();
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem)) return;

            ev.CanHurt = false;

            var color = GetRandomLaserColor();
            var laserColor = new Color(color.Red, color.Green, color.Blue, 0.1f) * 50;
            var direction = ev.Position - ev.Player.Position;
            var r_direction = ev.Player.CameraTransform.forward;
            var distance = direction.magnitude;
            var scale = new Vector3(LaserScale.x, distance * 0.5f, LaserScale.z);
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            var origin = ev.Player.CameraTransform.position;

            var laser = Primitive.Create(
                PrimitiveType.Cylinder,
                PrimitiveFlags.Visible,
                laserPos,
                rotation.eulerAngles,
                scale,
                true,
                laserColor
            );
            var hits = Physics.RaycastAll(origin, r_direction, 200f);
            if (hits.Length > 0)
            {
                // 거리순 정렬
                System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
    
                var firstHit = hits[0];
                var hub = firstHit.collider.GetComponentInParent<ReferenceHub>();
                var targetPlayer = Exiled.API.Features.Player.Get(hub);
                if (targetPlayer != null && targetPlayer != ev.Player)
                {
                    if (!targetPlayer.IsAlive)
                        return;
                    ev.Player.ShowHitMarker();
                    /*targetPlayer.Hurt(
                        attacker: ev.Player,
                        amount: 25f,
                        damageType: DamageType.E11Sr,
                        deathText:"레이저 관통"
                    );*/
                    targetPlayer.Hurt(new CustomReasonDamageHandler( "레이저 관통", 25f));
                }
            }

            Timing.CallDelayed(LaserVisibleTime, laser.Destroy);
        }

        private (float Red, float Green, float Blue) GetRandomLaserColor()
        {
            int randomColorR = new Random().Next(LaserColorRed.Count);
            int randomColorG = new Random().Next(LaserColorGreen.Count);
            int randomColorB = new Random().Next(LaserColorBlue.Count);
            return (randomColorR, randomColorG, randomColorB);
        }
    }
}