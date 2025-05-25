using System.Collections.Generic;
using System.Drawing;
using AdminToys;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using UnityEngine;
using Color = UnityEngine.Color;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunShotgun)]
    public class PlasmaShotgun : CustomWeapon
    {
        public override uint Id { get; set; } = 31;
        public override string Name { get; set; } = "<color=#00d0ff>PlasmaShotgun</color>";
        public override string Description { get; set; } = "This is a shotgun that uses a high-temperature <color=#00d0ff>plasma buckshot</color>";
        public override float Weight { get; set; } = 4.5f;
        public override ItemType Type { get; set; } = ItemType.GunShotgun;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczArmory,
                    Chance = 20,
                },
            }
        };
        public override float Damage { get; set; } = 0f;
        public override byte ClipSize { get; set; } = 4;
        
        
        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Player != null && ev.Attacker != ev.Player)
            {
                if (Check(ev.Attacker.CurrentItem))
                {
                    //ev.Player.EnableEffect<Decontaminating>(duration:5);
                    ev.Player.EnableEffect<Burned>(duration: 30);
                }
            }
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                Color glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                //var direction = ev.Position - ev.Player.Position;
                var direction = ev.Player.CameraTransform.forward.normalized;
                //var laserPos = ev.Player.Position + direction * 0.5f;
                var laserPos = ev.Player.CameraTransform.position + direction * 0.5f;
                var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                SpawnPrimitive.spawnPrimitive(ev.Player, PrimitiveType.Cube, rotation, laserPos, glowColor,25);
            }
            base.OnShot(ev);
        }
    }
}