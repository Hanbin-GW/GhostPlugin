using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Objects;
using PlayerStatsSystem;
using UnityEngine;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Mors : CustomWeapon
    {
        public override uint Id { get; set; } = 2;
        public override string Name { get; set; } = "<color=#94f6ff>MORS</color>";

        public override string Description { get; set; } =
            "<b>Bolt action railgun. Best in class handling</b>\nE-11-SR 의 개조형 버전이고 탄약수가 1발인 대신, 기존 앱실론 대미지의 <color=red>7.5 배</color> 입니다.";

        public override float Weight { get; set; } = 7f;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        [YamlIgnore] 
        public override float Damage { get; set; }

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.Surface,
                    Chance = 60
                },
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczArmory,
                    Chance = 30
                },
                new RoomSpawnPoint()
                {
                    Room = RoomType.LczArmory,
                    Chance = 40
                }
            }
        };

        public float DamageMultiplier { get; set; } = 7.5f;
        public override byte ClipSize { get; set; } = 1;

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != ev.Player && ev.DamageHandler.Base is FirearmDamageHandler firearmDamageHandler &&
                firearmDamageHandler.WeaponType == ev.Attacker.CurrentItem.Type)
                ev.Amount *= DamageMultiplier;
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            SpawnParticleSpark spark = new SpawnParticleSpark();
            Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
            spark.SpawnSpark(ev.Player, ev.Firearm.Base.transform.position,glowColor);
            ev.Firearm.Penetration = 500;
            ev.Firearm.DamageFalloffDistance = 300;
            base.OnShot(ev);
        }
    }
}