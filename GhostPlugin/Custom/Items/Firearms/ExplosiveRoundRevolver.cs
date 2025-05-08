using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.Grenades;
using JetBrains.Annotations;
using UnityEngine;
using YamlDotNet.Serialization;
using Player = Exiled.Events.Handlers.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunA7)]
    public class ExplosiveRoundRevolver : CustomWeapon
    {
        [YamlIgnore] 
        public override ItemType Type { get; set; } = ItemType.GunLogicer;
        public override uint Id { get; set; } = 21;
        public override string Name { get; set; } = "<color=#FF0000>Rocket Luncher</color>";
        public override string Description { get; set; } = "해당 무기는 폭탄을 생성합니다.";
        public override float Weight { get; set; } = 1f;

        [CanBeNull]
        public override SpawnProperties SpawnProperties { get; set; }

        public override float Damage { get; set; } = 0;
        public override byte ClipSize { get; set; } = 1;
        private float FuseTimes { get; set; } = 10f;
        //private float ScpGrenadeDamageMultiplier { get; set; } = .5f;
        public int MaxReload = 3;
        private int currentReload = 0;

        protected override void SubscribeEvents()
        {
            Player.Shot += OnShot;
        }

        protected override void UnsubscribeEvents()
        {
            Player.Shot -= OnShot;
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                currentReload++;
                if (currentReload == MaxReload)
                {
                    ev.Player.ShowHint(new string('\n',10) + $"<color=red>탄약없음</color>");
                    ev.Item.Destroy();
                }
                ev.Player.ShowHint(new string('\n',10) + $"남은 장전횟수: {MaxReload - currentReload}");
            }
            base.OnReloading(ev);
        }

        /*protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            Log.Debug($"VVUP Custom Items: Explosive Round Revolver, spawning grenade at {ev.Position}");
            ev.CanHurt = false;
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.Base.GetComponent<StickyGrenadeBehavior>();
            grenade.FuseTime = FuseTime;
            grenade.ScpDamageMultiplier = ScpGrenadeDamageMultiplier;
            grenade.SpawnActive(ev.Position);
        }*/
        
        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            Log.Debug($"VVUP Custom Items: Explosive Round Revolver, spawning grenade at {ev.Position}");
            ev.CanHurt = false;
            var grenade = new ImpactGrenade
            {
                FuseTime = FuseTimes,
                ExplodeOnCollision = true
            };

            Vector3 spawnPos = ev.Player.CameraTransform.position + ev.Player.CameraTransform.forward * 1f;
            var pickup = grenade.Throw(
                position: spawnPos,
                force: 0f,
                weight: 0.01f,
                fuseTime: grenade.FuseTime,
                grenadeType: grenade.Type,
                player: ev.Player
            );

            // pickup.Base는 ThrownProjectile
            if (pickup.Base.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = ev.Player.CameraTransform.forward * 100f; // 💥 총알처럼 직선으로 날아감
            }
        }
    }
}