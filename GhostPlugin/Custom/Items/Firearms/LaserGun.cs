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
using InventorySystem.Items.Firearms.Extensions;
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
        public float SpiralRadius { get; set; } = 0.15f;
        public float SpiralParticleSize { get; set; } = 0.15f;
        public float SpiralTurns { get; set; } = 3f;
        public float SpiralDensity { get; set; } = 10f;

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
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
            AttachmentName.RecoilReducingStock,
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
            //var laserColor = new Color(color.Red, color.Green, color.Blue, 0.1f) * 50;
            var laserColor = new Color(1f, 0f, 0f, 0.1f) * 50;
            /*var direction = ev.Position - ev.Player.Position;
            var r_direction = ev.Player.CameraTransform.forward;
            var distance = direction.magnitude;
            var scale = new Vector3(LaserScale.x, distance * 0.5f, LaserScale.z);
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);*/
            Vector3 origin;
            if (BarrelTipExtension.TryFindWorldmodelBarrelTip(ev.Player.CurrentItem.Serial, out BarrelTipExtension ext))
                origin = ext.WorldspacePosition;
            else
                origin = ev.Player.CameraTransform.position;
            Vector3 forward = ev.Position - origin;
            float distance = forward.magnitude;
            Vector3 direction = forward.normalized;
            Vector3 laserSize = new Vector3(0.05f, 0.05f, distance);
            Vector3 laserPosition = origin + forward * 0.5f;
            Quaternion quaternion = Quaternion.LookRotation(direction);
            var laser = Primitive.Create(
                PrimitiveType.Cylinder,
                PrimitiveFlags.Visible,
                laserPosition,
                quaternion.eulerAngles,
                laserSize,
                true,
                laserColor
            );
            var r_direction = ev.Player.CameraTransform.forward;
            //var hits = Physics.RaycastAll(origin, r_direction, 200f);
            int layerMask = ~0;
            var hits = Physics.RaycastAll(origin, direction, distance, layerMask, QueryTriggerInteraction.Ignore);

            if (hits.Length > 0)
            {
                // 거리순 정렬
                System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                foreach (var h in hits)
                {
                    var hub = h.transform.root.GetComponent<ReferenceHub>();
                    if (hub == null) continue;

                    var target_h = Exiled.API.Features.Player.Get(hub);
                    if (target_h == null || target_h == ev.Player || !target_h.IsAlive)
                        continue;

                    ev.Player.ShowHitMarker();
                    target_h.Hurt(new CustomReasonDamageHandler("레이저 관통", 25f));
                    break;
                }
                /*var firstHit = hits[0];
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
                    );
                    targetPlayer.Hurt(new CustomReasonDamageHandler( "레이저 관통", 25f));*/
                }

            Timing.RunCoroutine(LaserFade(laser));

            //Timing.CallDelayed(LaserVisibleTime, laser.Destroy);
            
            int minSegments = Mathf.CeilToInt(SpiralTurns * 16);
            int distanceBasedSegments = Mathf.CeilToInt(distance * SpiralDensity);
            int spiralSegments = Mathf.Max(minSegments, distanceBasedSegments);
            Vector3 target = ev.Position;
            Vector3 dir = (target - origin).normalized;

            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;

            if (right == Vector3.zero)
                right = Vector3.Cross(direction, Vector3.forward).normalized;
            Vector3 up = Vector3.Cross(direction, right).normalized;

            for (int i = 0; i < spiralSegments; i++)
            {
                float t = i / (float)spiralSegments;
                float nextT = (i + 1) / (float)spiralSegments;

                float angle = t * SpiralTurns * Mathf.PI * 2f;
                float nextAngle = nextT * SpiralTurns * Mathf.PI * 2f;

                Vector3 pointOnBeam = origin + direction * (distance * t);
                Vector3 nextPointOnBeam = origin + direction * (distance * nextT);

                Vector3 spiralOffset = (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * SpiralRadius;
                Vector3 nextSpiralOffset = (right * Mathf.Cos(nextAngle) + up * Mathf.Sin(nextAngle)) * SpiralRadius;

                Vector3 spiralPosition = pointOnBeam + spiralOffset;
                Vector3 nextSpiralPosition = nextPointOnBeam + nextSpiralOffset;

                Vector3 segmentDirection = (nextSpiralPosition - spiralPosition).normalized;
                float segmentLength = Vector3.Distance(spiralPosition, nextSpiralPosition);
                Vector3 segmentCenter = (spiralPosition + nextSpiralPosition) * 0.5f;

                Quaternion spiralRotation = Quaternion.LookRotation(segmentDirection);
                //Quaternion spiralRotation = Quaternion.FromToRotation(Vector3.up, dir);

                Vector3 cubeScale = new Vector3(SpiralParticleSize * 0.2f, SpiralParticleSize * 0.2f, segmentLength);

                Primitive spiralSegment = Primitive.Create(
                    PrimitiveType.Cube,
                    (PrimitiveFlags)2,
                    new Vector3?(segmentCenter),
                    new Vector3?(spiralRotation.eulerAngles),
                    new Vector3?(cubeScale),
                    true,
                    new Color?(laserColor)
                );
                Timing.RunCoroutine(LaserFade(spiralSegment));
            }
        }

        private (float Red, float Green, float Blue) GetRandomLaserColor()
        {
            int randomColorR = new Random().Next(LaserColorRed.Count);
            int randomColorG = new Random().Next(LaserColorGreen.Count);
            int randomColorB = new Random().Next(LaserColorBlue.Count);
            return (randomColorR, randomColorG, randomColorB);
        }
        
        IEnumerator<float> LaserFade(Primitive Laser)
        {
            Color color = new Color(Laser.Color.r, Laser.Color.g, Laser.Color.b, 1f);

            for (int i = 20; i > 0; i--)
            {
                float brightness = (300f * (i / 20f));
                Color newColor = new Color(color.r * brightness, color.g * brightness, color.b * brightness, 1f);

                Laser.Color = newColor;

                yield return Timing.WaitForSeconds(0.05f / 20f);
            }

            Laser.Destroy();
        }
    }
}