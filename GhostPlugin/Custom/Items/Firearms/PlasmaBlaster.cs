using System.Collections.Generic;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using GhostPlugin.Methods.Music;
using GhostPlugin.Methods.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    public class PlasmaBlaster : CustomWeapon, ICustomItemGlow
    {
        public override uint Id { get; set; } = 46;
        public override string Name { get; set; } = "Plasma Blaster";
        public override string Description { get; set; } = "It is a blaster with overwhelming firepower and powerful plasma technology.";
        public override float Weight { get; set; } = 4f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
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
            if(!Check(ev.Player.CurrentItem))
                return;
            string fileName = "Blaster.ogg";
            string path = Path.Combine(Plugin.Instance.EffectDirectory, fileName);
            float duration = API.Audio.AudioUtils.GetOggDurationInSeconds(path);
            MusicMethods.PlaySoundEffect(fileName,ev.Player,duration,7.5f);
            Color glowColor = new Color(1f, 0.5f, 0f, 0.1f) * 50;
            float intensity = 50f;
            switch (ev.Player.LeadingTeam)
            {
                case (LeadingTeam.FacilityForces):
                    Color baseColor = (Color) new Color32(71, 255, 255, 121);
                    glowColor = new Color(
                        baseColor.r * intensity,
                        baseColor.g * intensity,
                        baseColor.b * intensity,
                        baseColor.a
                    );
                    // glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                    break;
                case (LeadingTeam.ChaosInsurgency):
                    baseColor = (Color) new Color32(80, 255, 80, 121);
                    glowColor = new Color(
                        baseColor.r * intensity,
                        baseColor.g * intensity,
                        baseColor.b * intensity,
                        baseColor.a
                    );
                    break;
                case LeadingTeam.Anomalies:
                    baseColor = new Color32(255, 80, 80, 121);
                    glowColor = new Color(
                        baseColor.r * intensity,
                        baseColor.g * intensity,
                        baseColor.b * intensity,
                        baseColor.a
                    );
                    break;
            }
            var direction = ev.Position - ev.Player.Position;
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            SpawnPrimitive.spawnPrimitive(ev.Player, PrimitiveType.Cube, rotation, laserPos, glowColor,25, 60);
            ev.CanHurt = false;
            base.OnShot(ev);
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(71, 255, 255, 121);
        public float GlowRange { get; set; }
        public float GlowIntensity { get; set; }
    }
}