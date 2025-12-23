using System.Collections.Generic;
using AdminToys;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using GhostPlugin.Custom.Items.MonoBehavior;
using GhostPlugin.Methods.Objects;
using GhostPlugin.Methods.ToyUtils;
using MEC;
using Mirror;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCOM15)]
    public class AcidShooter : CustomWeapon, ICustomItemGlow
    {
        public override uint Id { get; set; } = 33;
        public override string Name { get; set; } = "Acid Shotter";
        public override string Description { get; set; } = "매우 강력한 독산이 들어간 12게이지를 가지고 있습니다.";
        public override float Weight { get; set; } = 3.5f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczNuke,
                    Chance = 20
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
                Color glowColor = new Color(0.0f, 1.0f, 0.0f, 0.1f) * 50f;

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
                SpawnPrimitive.spawnPrimitivesNoGravity(ev.Player, 10, rotation, laserPos, glowColor,2,25, typeof(PoisonBulletCollision));
            }
            base.OnShot(ev);
        }
        protected override void OnChanging(ChangingItemEventArgs ev)
        {
            var light = LightUtils.SpawnLight(ev.Player,ev.Player.Position + Vector3.up, Quaternion.identity, 4f, 12f, Color.green);
            if (light == null)
                return;
            NetworkServer.Spawn(light.gameObject);
            Timing.RunCoroutine(FollowPlayerLight(ev.Player, light));
            Timing.CallDelayed(0.2f, () =>
            {
                var light = LightUtils.SpawnLight(ev.Player, ev.Player.Position + Vector3.up, Quaternion.identity, 4f, 12f, Color.green);
                if (light == null)
                {
                    Log.Error("Light spawn failed in OnAcquired");
                    return;
                }

                Timing.RunCoroutine(FollowPlayerLight(ev.Player, light));
            });
            base.OnChanging(ev);
        }
        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            base.OnDroppingItem(ev);
        }

        private IEnumerator<float> FollowPlayerLight(Player player, LightSourceToy light)
        {
            while (player.IsAlive && player.IsConnected)
            {
                if (light == null || light.gameObject == null)
                    break;
                
                if (!Check(player.CurrentItem))
                    break;

                light.transform.position = player.Position + Vector3.up;
                light.transform.rotation = Quaternion.identity;
                yield return Timing.WaitForSeconds(0.1f);
            }

            if (light != null && light.gameObject != null)
                NetworkServer.Destroy(light.gameObject);
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(128, 255, 0, 52);
        public float GlowRange { get; set; }
        public float GlowIntensity { get; set; }
    }
}