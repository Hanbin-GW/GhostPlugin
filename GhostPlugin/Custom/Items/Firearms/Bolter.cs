using System;
using CameraShaking;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Mirror;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunA7)]
    public class Bolter : CustomWeapon
    { 
        public override uint Id { get; set; } = 32;
        public override string Name { get; set; } = "Bolter";
        public override string Description { get; set; } = "강력한 자동 볼트액션 소총입니다";
        public override float Weight { get; set; } = 15;
        public override ItemType Type { get; set; } = ItemType.GunA7;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; } = 45;
        public override byte ClipSize { get; set; } = 50;
        
        [SyncVar]
        private RecoilSettings syncedRecoil = new RecoilSettings
        {
            ZAxis = 5f,
            UpKick = 5f,
            SideKick = 5f,
            FovKick = 5f,
        };
        
        protected override void OnShot(ShotEventArgs ev)
        {
            if (ev.Player.IsHost) // 서버에서만 실행
            {
                syncedRecoil = new RecoilSettings
                {
                    ZAxis = 5f,
                    UpKick = 5f,
                    SideKick = 5f,
                    FovKick = 5f,
                };

                RpcSyncRecoil(ev.Player.Id, syncedRecoil.ZAxis, syncedRecoil.UpKick, syncedRecoil.SideKick, syncedRecoil.FovKick);
            }
            ev.Firearm.Recoil = syncedRecoil;
            base.OnShot(ev);
        }

        private void Onaimimg(AimingDownSightEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.Firearm.Inaccuracy = 0;
            }
        }
        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Player.IsHost)
            {
                syncedRecoil = new RecoilSettings
                {
                    ZAxis = 5f,
                    UpKick = 5f,
                    SideKick = 5f,
                    FovKick = 5f,
                };

                RpcSyncRecoil(ev.Player.Id, syncedRecoil.ZAxis, syncedRecoil.UpKick, syncedRecoil.SideKick, syncedRecoil.FovKick);
            }
            ev.Firearm.Recoil = syncedRecoil;
            ev.Firearm.Inaccuracy = 0;
            base.OnShooting(ev);
        }
        
        [ClientRpc]
        private void RpcSyncRecoil(int playerId, float zAxis, float upKick, float sideKick, float fovKick)
        {
            if (!Player.TryGet(playerId, out Player player))
                return;

            if (player.CurrentItem is not Firearm firearm)
                return;

            firearm.Recoil = new RecoilSettings
            {
                ZAxis = zAxis,
                UpKick = upKick,
                SideKick = sideKick,
                FovKick = fovKick
            };
        }
    }
}