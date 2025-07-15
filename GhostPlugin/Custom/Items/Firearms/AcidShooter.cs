using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using GhostPlugin.Methods.ToyUtils;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCOM15)]
    public class AcidShooter : CustomWeapon
    {
        public override uint Id { get; set; } = 33;
        public override string Name { get; set; } = "Acid Shotter";
        public override string Description { get; set; } = "It has 12 gauges with very strong toxic acid.";
        public override float Weight { get; set; } = 3.5f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczNuke,
                    Chance = 100
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.GunCOM15;
        public override float Damage { get; set; } = 19f;
        public override byte ClipSize { get; set; } = 5;

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                Color glowColor = new Color(0.5f, 1.0f, 0.0f, 0.1f) * 50f;

                /*List<PrimitiveObjectToy> bullets = PlasmaCube.SpawmSparkAmmos(ev.Player, ev.Firearm.Base.transform.position, 10, 25f, 0.5f, glowColor);

                foreach (var bullet in bullets)
                {
                    var collision = bullet.gameObject.AddComponent<PoisonBulletCollision>();
                    collision.Initialize(5,ev.Player);
                }*/
                var direction = ev.Position - ev.Player.Position;
                var laserPos = ev.Player.Position + direction * 0.25f;
                var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                //PlasmaCube.SpawmSparkBuckshot(ev.Player, ev.Firearm.Base.transform.position,13,15f,0.05f,glowColor); 
                SpawnPrimitive.spawnPrimitivesNoGravity(ev.Player, 10, rotation, laserPos, glowColor,5,25, typeof(PoisonBulletCollision));
            }
            base.OnShot(ev);
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if(!Check(player.CurrentItem))
                return;
            base.OnAcquired(player, item, displayMessage);
            var light = LightUtils.SpawnLight(player.Position + Vector3.up, Quaternion.identity, 4f, 12f, Color.red);
            if (light == null)
                return;
            Timing.RunCoroutine(FollowPlayerLight(player, light));
        }
        
        private IEnumerator<float> FollowPlayerLight(Player player, LightSourceToy light)
        {
            while (player.IsAlive && player.IsConnected)
            {
                if (!Check(player.CurrentItem))
                    break;

                light.transform.position = player.Position + Vector3.up;
                yield return Timing.WaitForSeconds(0.1f);
            }

            Object.Destroy(light);
        }

    }
}