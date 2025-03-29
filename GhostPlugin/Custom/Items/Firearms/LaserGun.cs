using System.Collections.Generic;
using System.ComponentModel;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using MEC;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;
using Random = System.Random;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class LaserGun : CustomWeapon
    {
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        public override uint Id { get; set; } = 60;
        public override string Name { get; set; } = "<color=#FF0000>X-57 Helios Beam</color>";
        public override string Description { get; set; } = "It fires lasers!";
        public override float Weight { get; set; } = 2;
        public override byte ClipSize { get; set; } = 10;

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
        [Description("The red color of the laser, values must be between 0 and 1")]
        public List<float> LaserColorRed { get; set; } = new List<float>()
        {
            0.86f, 
            1, 
            0,
            0.55f,
            0.97f,
        };
        [Description("The green color of the laser, values must be between 0 and 1")]
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
        [Description("The blue color of the laser, values must be between 0 and 1")]
        public List<float> LaserColorBlue { get; set; } = new List<float>()
        {
            0.24f,
            0,
            0.31f,
            1,
            0.96f,
        };


        [Description("How long does the laser stay on the screen")]
        public float LaserVisibleTime { get; set; } = 0.5f;
        [Description("How big is the laser")]
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
        private void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.CanHurt = false;
            Log.Debug($"VVUP Custom Items: Laser Gun, spawning laser going from {ev.Player.Position} to {ev.Position}");
            //var color = GetRandomLaserColor();
            //var laserColor = new Color(color.Red, color.Green, color.Blue);
            var laserColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
            var direction = ev.Position - ev.Player.Position;
            var distance = direction.magnitude;
            var scale = new Vector3(LaserScale.x, distance * 0.5f, LaserScale.z);
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            Log.Debug($"VVUP Custom Items: Laser Gun, Laser Info: Position: {laserPos}, Rotation: {rotation.eulerAngles}, Color: {laserColor}");
            var laser = Primitive.Create(PrimitiveType.Cylinder, PrimitiveFlags.Visible, laserPos, rotation.eulerAngles,
                scale, true, laserColor);
            var bulletCollision = laser.Base.gameObject.AddComponent<BulletCollision>();
            bulletCollision.Initialize(100, ev.Player);
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