using AdminToys;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class PlasmaEmitter : CustomWeapon
    {
        public override uint Id { get; set; } = 25;
        public override string Name { get; set; } = "PlasmaEmitter";
        public override string Description { get; set; } = "25 Ammo plasma rifles.\na Serpents Hand-only itam";
        public override float Weight { get; set; } = 5.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 30;
        public override ItemType Type { get; set; } = ItemType.GunCOM18;

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                Color glowColor = new Color(1.0f, 0.5f, 0.0f, 0.1f) * 50f;
                var direction = ev.Position - ev.Player.Position;
                var laserPos = ev.Player.Position + direction * 0.5f;
                var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                SpawnPrimitive.spawnPrimitive(ev.Player, PrimitiveType.Cube, rotation, laserPos, glowColor, 80);
            }
            base.OnShot(ev);
        }
    }
}