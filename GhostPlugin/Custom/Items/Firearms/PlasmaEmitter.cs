using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.MER;
using GhostPlugin.Methods.Objects;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class PlasmaEmitter : CustomWeapon
    {
        public override uint Id { get; set; } = 25;
        public override string Name { get; set; } = "<color=#edd900>하이브리드 플라즈마 권총</color>";
        public override string Description { get; set; } = "15발 플라즈마 권총입니다.\n조준시 <color=#edd900>에너지 방패가</color> 생성됩니다!";
        public override float Weight { get; set; } = 5.5f;
        public SchematicObject obj = null;
        private Dictionary<int, float> shieldCooldowns = new();

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside049Armory,
                    Chance = 70,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideLczArmory,
                    Chance = 40,
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Zone = ZoneType.HeavyContainment,
                    Chance = 80,
                    Type = LockerType.Misc,
                    Offset = new Vector3(0, 1.1f, 0),
                }
            }
        };
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 15;
        public override ItemType Type { get; set; } = ItemType.GunCOM18;

        private void OnAimDownSight(AimingDownSightEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            int playerId = ev.Player.Id;
            if (shieldCooldowns.TryGetValue(playerId, out float lastDisableTime))
            {
                if (Time.time - lastDisableTime < 5f)
                {
                    ev.Player.ShowHint("<color=red>쉴드가 재충전 중입니다... (5초 쿨다운)</color>", 2);
                    return;
                }
            }
            if (ev.AdsIn)
            {
                obj = ObjectManager.SpawnObject("Shield", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, ev.Player.Transform.rotation);
            }
            else
            {
                if (obj != null)
                {
                    ObjectManager.RemoveObject(obj);
                    obj = null;
                }
                shieldCooldowns[playerId] = Time.time;

            }
        }
        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem) && ev.Player.IsAimingDownWeapon)
            {
                ev.Player.ShowHint("조준 사격 불가능");
                return;
            }

            Color glowColor = new Color();
            Color baseColor;
            float intensity = 50f;
            switch (ev.Player.LeadingTeam)
            {
                case (LeadingTeam.FacilityForces):
                    baseColor = new Color32(0,255,255,121);
                    glowColor = new Color(baseColor.r * intensity,baseColor.g * intensity, baseColor.b * intensity, baseColor.a);
                    break;
                case (LeadingTeam.ChaosInsurgency):
                    baseColor = new Color32(255,178,0,121);
                    glowColor = new Color(
                        baseColor.r * intensity, 
                        baseColor.g * intensity,
                        baseColor.b * intensity,
                        baseColor.a);
                    break;
                case LeadingTeam.Anomalies:
                    baseColor = new Color32(255, 0, 0, 121);
                    glowColor = new Color(
                        baseColor.r * intensity,
                        baseColor.g * intensity,
                        baseColor.b * intensity,
                        baseColor.a);
                    break;
            }
            var direction = ev.Position - ev.Player.Position;
            var laserPos = ev.Player.Position + direction * 0.5f;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            SpawnPrimitive.spawnPrimitive(ev.Player, PrimitiveType.Cube, rotation, laserPos, glowColor,25, 70);
            ev.CanHurt = false;
            base.OnShot(ev);
        }
        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (obj != null)
            {
                ObjectManager.RemoveObject(obj);
                obj = null;
            }
            shieldCooldowns[ev.Player.Id] = Time.time;

            base.OnReloading(ev);
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.AimingDownSight += OnAimDownSight;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.AimingDownSight -= OnAimDownSight;
            base.UnsubscribeEvents();
        }
    }
}