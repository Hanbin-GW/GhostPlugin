using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunLogicer)]
    public class MachineGun : CustomWeapon
    {
        public override uint Id { get; set; } = 50;
        public override string Name { get; set; } = "<color=#ffef94>RAAL MG</color>";
        public override string Description { get; set; } = ".300 구경을 사용한 피해량과 반동이 약간 높은 기관총입니다.";
        public override float Weight { get; set; } = 6f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; } = 38.5f;
        public override ItemType Type { get; set; } = ItemType.GunLogicer;
        public override byte ClipSize { get; set; } = 75;

        protected override void OnShot(ShotEventArgs ev)
        {
            if (ev.Player == null || !ev.Player.IsAlive)
                return;

            float recoilX = Random.Range(-15f, 15f);  // 좌우 반동
            float recoilY = Random.Range(16f, 20f);      // 상하 반동

            Vector3 currentRotation = ev.Player.CameraTransform.eulerAngles;

            // 카메라 상하 각도를 반동 값으로 조정, 클램핑 적용
            currentRotation.x = Mathf.Clamp(currentRotation.x - recoilY, -90f, 90f);  // 상하 각도 제한
            currentRotation.y += recoilX;  // 좌우 회전 추가

            ev.Player.CameraTransform.eulerAngles = currentRotation;

            Log.Debug($"Player {ev.Player.Nickname} shot using {Name} with recoil X:{recoilX:F2} Y:{recoilY:F2}");

            base.OnShot(ev);
        }
        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            base.OnReloading(ev);
        }
    }
}