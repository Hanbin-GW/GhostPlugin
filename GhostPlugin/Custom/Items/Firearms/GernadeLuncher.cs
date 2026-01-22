using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.Objects;
using UnityEngine;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunRevolver)]
    public class GernadeLuncher : CustomWeapon
    {
        public override uint Id { get; set; } = 5;
        public override string Name { get; set; } = "M79";
        public override string Description { get; set; } = "TESTING IN PROGRESS\nDon'T USE IT!";
        public override float Weight { get; set; } = 4f;
        public override ItemType Type { get; set; } = ItemType.GunRevolver;
        public override byte ClipSize { get; set; } = 1;
        public override SpawnProperties SpawnProperties { get; set; }
        [YamlIgnore] 
        public override float Damage { get; set; } = 0;

        protected override void OnShooting(ShootingEventArgs ev)
        {
            base.OnShooting(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;

                SpawnParticleSpark spark = new SpawnParticleSpark();
                Color color = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                Vector3 position = ev.Player.Position + ev.Player.Transform.forward * 3.0f;

                spark.SpawnGrenade(ev.Player, position, 10, 0, color);
        
                //ar bulletCollision = bullet.gameObject.AddComponent<BulletExplosion>();
                //bulletCollision.Initialize(ev.Player);

                /*CharacterController playerController = ev.Player.GameObject.GetComponent<CharacterController>();
                Collider bulletCollider = bullet.gameObject.GetComponent<Collider>();

                if (playerController != null && bulletCollider != null)
                {
                    Physics.IgnoreCollision(bulletCollider, playerController, true);
                    Log.Info("발사자와 총알 간 충돌 무시 처리 완료.");
                }*/
            }

            base.OnShot(ev);
        }

    }
}