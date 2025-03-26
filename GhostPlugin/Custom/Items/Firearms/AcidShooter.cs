using AdminToys;
using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCOM15)]
    public class AcidShooter : CustomWeapon
    {
        public override uint Id { get; set; } = 33;
        public override string Name { get; set; } = "Acid Shotter";
        public override string Description { get; set; } = "매우 강력한 독산이 들어간 12게이지를 가지고 있습니다.";
        public override float Weight { get; set; } = 3.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunCOM15;
        public override float Damage { get; set; } = 19f;
        public override byte ClipSize { get; set; } = 5;

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                PlasmaCube spark = new PlasmaCube(); 
                Color glowColor = new Color(0.0f, 1.0f, 0.0f, 0.1f) * 50f;
                PrimitiveObjectToy bullet = spark.SpawmSparkAmmo(ev.Player, ev.Firearm.Base.transform.position,10,25f,0.5f,glowColor); 
                var bulletCollision = bullet.gameObject.AddComponent<PoisonBulletCollision>();
                bulletCollision.Initialize(ev.Player); 
                //ev.Player.EnableEffect<Decontaminating>(duration:5);
                ev.Target.EnableEffect<Burned>(duration: 30);
            }
            base.OnShot(ev);
        }
    }
}