using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using PlayerStatsSystem;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Ballista : CustomWeapon, ICustomItemGlow
    {
        public override byte ClipSize { get; set; } = 1;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
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

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.Laser,
            AttachmentName.LowcapMagAP,
            AttachmentName.CarbineBody,
            AttachmentName.NightVisionSight,
            AttachmentName.LightweightStock,
        };
        public override float Damage { get; set; } = 0;
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
            ev.Player.ShowHint("You cannot able to change attachment in this items", 3);
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            Player.Shot += OnShot;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            Player.Shot -= OnShot;
            base.UnsubscribeEvents();
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem)) return;
            ev.CanHurt = false;
            ev.Firearm.Inaccuracy = 0f;

            var laserColor = new Color(1f, 0.3f, 0.2f, 0.1f) * 50f;
            var origin = ev.Player.CameraTransform.position + ev.Player.CameraTransform.forward * 0.3f;
            var direction = ev.Player.CameraTransform.forward;
            float distance = 200f;

            // Visual Effects (unconditionally straight 200 distances)
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
            // Damage: Detecting people who have been laser-hit
            var hits = Physics.RaycastAll(origin, direction, distance);
            foreach (var hit in hits)
            {
                var hub = hit.collider.GetComponentInParent<ReferenceHub>();
                var target = Exiled.API.Features.Player.Get(hub);

                if (target == null || target == ev.Player || damagedPlayers.Contains(target))
                    continue;

                if (!target.IsAlive || target.LeadingTeam == ev.Player.LeadingTeam)
                    continue;

                damagedPlayers.Add(target); // Avoid duplication
                ev.Player.ShowHitMarker(1.5f);
                target.Hurt(new CustomReasonDamageHandler("레이저 관통", 100));
            }
            Timing.CallDelayed(LaserVisibleTime, laser.Destroy);
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = Color.red;
    }
}