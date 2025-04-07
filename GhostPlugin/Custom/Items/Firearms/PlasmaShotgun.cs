using System.Collections.Generic;
using System.Drawing;
using AdminToys;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using UnityEngine;
using Color = UnityEngine.Color;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunShotgun)]
    public class PlasmaShotgun : CustomWeapon
    {
        public override uint Id { get; set; } = 31;
        public override string Name { get; set; } = "<color=#00d0ff>PlasmaShotgun</color>";
        public override string Description { get; set; } = "데미지가 높은 고열의 <color=#00d0ff>플라즈마 벅샷</color>을 사용하는 샷건입니다.";
        public override float Weight { get; set; } = 4.5f;
        public override ItemType Type { get; set; } = ItemType.GunShotgun;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczArmory,
                    Chance = 20,
                },
            }
        };
        public override float Damage { get; set; } = 0f;
        public override byte ClipSize { get; set; } = 4;
        
        
        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Player != null && ev.Attacker != ev.Player)
            {
                if (Check(ev.Attacker.CurrentItem))
                {
                    //ev.Player.EnableEffect<Decontaminating>(duration:5);
                    ev.Player.EnableEffect<Burned>(duration: 30);
                }
            }
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                SpawnParticleSpark spark = new SpawnParticleSpark();
                /*Color color = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                spark.SpawnSpark(ev.Player, ev.Firearm.Base.transform.position,color);*/
                //PrimitiveObjectToy bullet = spark.SpawmSparkAmmoNoSpread(ev.Player, ev.Firearm.Base.transform.position, 25f,color ,0.5f);
                PrimitiveObjectToy bullet = spark.SpawnEnergyGuage(ev.Player, ev.Firearm.Base.transform.position);
                var bulletCollision = bullet.gameObject.AddComponent<BulletCollision>();
                bulletCollision.Initialize(125, ev.Player);
            }
            base.OnShot(ev);
        }
    }
}