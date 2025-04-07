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
    [CustomItem(ItemType.GunRevolver)]
    public class GernadeLuncher : CustomWeapon
    {
        public override uint Id { get; set; } = 5;
        public override string Name { get; set; } = "M79 (유탄 발사기)";
        public override string Description { get; set; } = "수류탄을 발사하는 리볼버 입니다!";
        public override float Weight { get; set; } = 4f;
        public override ItemType Type { get; set; } = ItemType.GunRevolver;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; }

        protected override void OnShot(ShotEventArgs ev)
        {
            if(!Check(ev.Player.CurrentItem))
                return;
            SpawnParticleSpark spark = new SpawnParticleSpark();
            Color color = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
            PrimitiveObjectToy bullet = spark.SpawmSparkAmmo(ev.Player, ev.Firearm.Base.transform.position,1,50,0,color);
            var bulletCollision = bullet.gameObject.AddComponent<BulletExplosion>();
            bulletCollision.Initialize(ev.Player);
            base.OnShot(ev);
        }
    }
}