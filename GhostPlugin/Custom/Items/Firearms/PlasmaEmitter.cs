using AdminToys;
using Exiled.API.Enums;
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
        public override string Description { get; set; } = "25발 플라즈마 소총입니다.\n뱀의손 전용 아이탬";
        public override float Weight { get; set; } = 5.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 30;
        public override ItemType Type { get; set; } = ItemType.GunCOM18;

        protected override void OnShot(ShotEventArgs ev)
        {
            /*float recoilX = Random.Range(-20f, 30f);  // 좌우 반동
            float recoilY = Random.Range(22f, 26f);      // 상하 반동

            Vector3 currentRotation = ev.Player.CameraTransform.eulerAngles;

            // 카메라 상하 각도를 반동 값으로 조정, 클램핑 적용
            currentRotation.x = Mathf.Clamp(currentRotation.x - recoilY, -90f, 90f);  // 상하 각도 제한
            currentRotation.y += recoilX;  // 좌우 회전 추가

            ev.Player.CameraTransform.eulerAngles = currentRotation;*/
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