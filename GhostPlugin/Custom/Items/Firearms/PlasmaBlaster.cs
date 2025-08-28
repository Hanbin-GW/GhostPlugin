using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    public class PlasmaBlaster : CustomWeapon
    {
        public override uint Id { get; set; } = 46;
        public override string Name { get; set; } = "Plasma Blaster";
        public override string Description { get; set; } = "It's a blaster with overwhelming firepower and powerful plasma technology";
        public override float Weight { get; set; } = 5f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside049Armory,
                    Chance = 100,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideLczArmory,
                    Chance = 40,
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.GunCrossvec;
        public override byte ClipSize { get; set; } = 20;

        protected override void OnShot(ShotEventArgs ev)
        {
            Color glowColor = new Color(1f, 0.5f, 0f, 0.1f) * 50;
            switch (ev.Player.LeadingTeam)
            {
                case (LeadingTeam.FacilityForces):
                    glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                    break;
                case (LeadingTeam.ChaosInsurgency):
                    glowColor = new Color(0.1f, 1f, 0.1f, 0.1f) * 50;
                    break;
                case LeadingTeam.Anomalies:
                    glowColor = new Color(1f, 0f, 0f, 0.1f) * 50;
                    break;
            }
            var direction = ev.Position - ev.Player.Position;
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            SpawnPrimitive.spawnPrimitive(ev.Player, PrimitiveType.Cube, rotation, laserPos, glowColor,25);
            ev.CanHurt = false;
            base.OnShot(ev);
        }
    }
}