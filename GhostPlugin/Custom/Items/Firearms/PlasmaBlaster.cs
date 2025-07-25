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
        public override string Name { get; set; } = "플라스마 소총";
        public override string Description { get; set; } = "태스트 버전";
        public override float Weight { get; set; } = 4f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 35;

        protected override void OnShot(ShotEventArgs ev)
        {
            Color glowColor = new Color(1f, 0.5f, 0f, 0.1f) * 50;
            switch (ev.Player.LeadingTeam)
            {
                case (LeadingTeam.FacilityForces):
                    glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                    break;
                case (LeadingTeam.ChaosInsurgency):
                    glowColor = new Color(1f, 0.5f, 0f, 0.1f) * 50;
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