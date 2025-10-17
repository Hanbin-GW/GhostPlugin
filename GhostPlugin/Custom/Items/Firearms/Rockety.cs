using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Components;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using System.ComponentModel;
using Exiled.API.Features.Items;
using GhostPlugin.Custom.Items.MonoBehavior;
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
        public override string Description { get; set; } = "3번 사용할수 있는 사정거리가 넓은 로캣포입니다!\n(폭탄이 포물선이 아닌 직진으로 갑니다)";
        public override float Weight { get; set; } = 21f;
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GunLogicer;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside127Lab,
                    Chance = 20,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside106Primary,
                    Chance = 20,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside096,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideSurfaceNuke,
                    Chance = 15,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidChamber,
                    Chance = 20
                }
            }
        };
        private readonly Dictionary<uint, int> _charges = new();
        private const int MaxCharges = 3;
        public override byte ClipSize { get; set; } = 1;
        [Description("Sometimes you're able to get more than what ClipSize is set to when reloading, if this is set to true, it will check and correct the ammo count")]
        public bool FixOverClipSizeBug { get; set; } = true;

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            _charges[item.Serial] = MaxCharges;
            base.OnAcquired(player, item, displayMessage);
        }
        private int GetCharges(Item item) =>
            (item != null && _charges.TryGetValue(item.Serial, out var c)) ? c : 0;
        private void SetCharges(Item item, int value)
        {
            if (item == null) return;
            _charges[item.Serial] = Mathf.Max(0, value);
        }
        protected override void OnReloaded(ReloadedWeaponEventArgs ev)
        {
            // 탄창 강제 동기화
            ev.Firearm.MagazineAmmo = ClipSize;

            // ✅ 사용 횟수 감소
            var left = GetCharges(ev.Item) - 1;
            SetCharges(ev.Item, left);

            // 다 쓰면 아이템 파괴
            if (left <= 0)
            {
                // 딕셔너리 정리(선택 사항)
                _charges.Remove(ev.Item.Serial);
                ev.Item.Destroy();
            }
        }
        
        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            // 남은 횟수가 0이면 장전 금지
            if (GetCharges(ev.Item) <= 0)
            {
                ev.IsAllowed = false;
                return;
            }

            ev.IsAllowed = true;
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.Player.CurrentItem is Firearm firearm)
            {
                if (firearm.MagazineAmmo > ClipSize && FixOverClipSizeBug)
                    firearm.MagazineAmmo = ClipSize;

                firearm.MagazineAmmo -= 1;
            }

            // 수류탄 생성 후 직진화
            var proj = ev.Player.ThrowGrenade(ProjectileType.FragGrenade, false).Projectile;
            MakeStraight(ev.Player, proj.Base);
        }

        private void MakeStraight(Player player, ThrownProjectile proj)
        {
            if (proj == null) return;
            var rb = proj.GetComponent<Rigidbody>();
            if (rb == null) return;

            rb.useGravity = false;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            proj.gameObject.AddComponent<CollisionHandler>().Init(player.GameObject, proj);

            const float straightSpeed = 20f;

            // 카메라 전방에서 Y 성분 제거 → 완전 수평 방향
            Vector3 dir = player.CameraTransform.forward;
            dir.y = 0f;
            dir = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.forward;

            // 1) 최초 속도 부여
            rb.velocity = dir * straightSpeed;

            // 2) 네트코드/다른 스크립트 간섭을 이기기 위해 매 FixedUpdate 때 속도 고정
            proj.gameObject.AddComponent<StraightFlight>().Init(rb, dir, straightSpeed, lockHorizontal:true);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }
    }
}
