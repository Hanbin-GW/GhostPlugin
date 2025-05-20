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
        public override string Description { get; set; } = "25 Ammo plasma rifles.\na snake's hand-only itam";
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
            SpawmPlasma spawmPlasma = new SpawmPlasma();
            spawmPlasma.SpawnPlasma(ev.Player);
            ev.CanHurt = false;
            base.OnShot(ev);
        }
    }
}