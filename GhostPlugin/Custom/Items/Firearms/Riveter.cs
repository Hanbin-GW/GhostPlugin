using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using YamlDotNet.Serialization;
using GhostPlugin.Methods.Objects;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Riveter : CustomWeapon
    {
        public override uint Id { get; set; } = 51;
        public override string Name { get; set; } = "Riveter";
        public override string Description { get; set; } = "It's an AR, but it's a continuous shotgun that uses <color=#ebc934>.410 Gauge Dragon Breath</color>";
        public override float Weight { get; set; } = 7.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 10;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        [YamlIgnore] 
        public override float Damage { get; set; }
        
        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                Color glowColor = new Color(1.0f, 0.0f, 0.0f, 0.1f) * 50f;
                //var direction = ev.Position - ev.Player.Position;
                var direction = ev.Player.CameraTransform.forward.normalized;
                //var laserPos = ev.Player.Position + direction * 0.5f;
                var laserPos = ev.Player.CameraTransform.position + direction * 0.5f;
                var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                SpawnPrimitive.spawnPrimitives(ev.Player, 15, rotation, laserPos, glowColor,5,20);
            }
            base.OnShot(ev);
        }
    }
}