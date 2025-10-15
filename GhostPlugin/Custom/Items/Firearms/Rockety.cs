using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Components;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using System.ComponentModel;
using UnityEngine;
using YamlDotNet.Serialization;
using Firearm = Exiled.API.Features.Items.Firearm;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunLogicer)]
    public class Rockety : CustomWeapon
    {
        public override uint Id { get; set; } = 71;
        public override string Name { get; set; } = "HE-1";
        public override string Description { get; set; } = "3번 사용할수 있는 로캣포입니다!";
        public override float Weight { get; set; } = 21f;
        public override ItemType Type { get; set; } = ItemType.GunLogicer;
        public override SpawnProperties SpawnProperties { get; set; }
        public int Count = 3;
        public override byte ClipSize { get; set; } = 1;
        [Description("Sometimes you're able to get more than what ClipSize is set to when reloading, if this is set to true, it will check and correct the ammo count")]
        public bool FixOverClipSizeBug { get; set; } = true;

        protected override void OnReloaded(ReloadedWeaponEventArgs ev)
        {
            Log.Debug($"VVUP Custom Items: Grenade Launcher Impact: {ev.Player.Nickname} reloaded the Grenade Launcher Impact setting Magazine Ammo to {ClipSize}.");
            ev.Firearm.MagazineAmmo = ClipSize;
            if (Count == 0)
                ev.Item.Destroy();
            --Count;
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            ev.IsAllowed = false;
            if (ev.Player.CurrentItem is Firearm firearm)
            {
                if (firearm.MagazineAmmo > ClipSize && FixOverClipSizeBug)
                {
                    Log.Debug("VVUP Custom Items: Grenade Launcher Impact: Fixing ammo count due to over clip size bug");
                    firearm.MagazineAmmo = ClipSize;
                }
                firearm.MagazineAmmo -= 1;
            }
            // 1) 수류탄 생성
            var proj = ev.Player.ThrowGrenade(ProjectileType.FragGrenade, false).Projectile;
            MakeStraight(ev.Player, proj.Base);

        }

        private void MakeStraight(Player player, ThrownProjectile proj)
        {
            if (proj == null) return;
            var rb = proj.GetComponent<Rigidbody>();
            if (rb == null) return;

            // 중력 끄고, 회전저항 제거, 속도 직접 세팅
            rb.useGravity = false;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.angularVelocity = Vector3.zero;
            proj.gameObject.AddComponent<CollisionHandler>().Init(player.GameObject, proj);

            // 플레이어 카메라 전방으로 직선 속도 부여
            const float straightSpeed = 20f;
            Vector3 forward = player.CameraTransform.forward.normalized;
            rb.velocity = forward * straightSpeed;
        }

    }
}
