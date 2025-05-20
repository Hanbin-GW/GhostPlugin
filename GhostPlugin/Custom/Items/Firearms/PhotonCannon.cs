using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class PhotonCannon : CustomWeapon
    {
        public override uint Id { get; set; } = 27;
        public override string Name { get; set; } = "Photon Cannon";
        public override string Description { get; set; } = "20 photon cannons.\nItam for Snake's Hand Overseer";
        public override float Weight { get; set; } = 5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; } = 400;
        public override byte ClipSize { get; set; } = 20;
        public override ItemType Type { get; set; } = ItemType.ParticleDisruptor;

        protected override void OnShot(ShotEventArgs ev)
        {
            float recoilX = Random.Range(-10f, 10f);  // 좌우 반동
            float recoilY = Random.Range(6f, 10f);      // 상하 반동

            Vector3 currentRotation = ev.Player.CameraTransform.eulerAngles;

            // 카메라 상하 각도를 반동 값으로 조정, 클램핑 적용
            currentRotation.x = Mathf.Clamp(currentRotation.x - recoilY, -45f, 45f);  // 상하 각도 제한
            currentRotation.y += recoilX;  // 좌우 회전 추가

            ev.Player.CameraTransform.eulerAngles = currentRotation;

            base.OnShot(ev);
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.Player.EnableEffect<Scp1853>(intensity:4, duration: 10);
        }
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReloadingWeapon += OnReloading;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReloadingWeapon -= OnReloading; 
            base.UnsubscribeEvents();
        }
    }
}