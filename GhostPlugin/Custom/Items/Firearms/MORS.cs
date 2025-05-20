using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.Objects;
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
            "<b>Bolt action railgun. Best in class handling</b>\nIt is a modified version of the nE-11-SR and instead of having one ammunition, it is <color=red>7.5 times </color> of the existing Apsilon damage.";

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
            if (!Check(ev.Player.CurrentItem))
                return;
            /*SpawnLaserObject spawnLaserObject = new SpawnLaserObject();
            spawnLaserObject.SpawnLaser(ev.Player);*/
            ev.Firearm.Penetration *= DamageMultiplier;
            ev.Firearm.DamageFalloffDistance = 300;
            base.OnShot(ev);
        }
    }
}