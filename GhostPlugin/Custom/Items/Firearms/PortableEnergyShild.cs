using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.MER;
using InventorySystem.Items.Firearms.Attachments;
using ProjectMER.Features.Objects;
using UnityEngine;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCrossvec)]
    public class PortableEnergyShild : CustomWeapon
    {
        public override uint Id { get; set; } = 54;
        public override string Name { get; set; } = "<color=blue>Portable Energy Shield</color>";
        public override string Description { get; set; } = "The Energy Shield is activate in ADS";
        public override float Weight { get; set; } = 2.3f;
        [YamlIgnore]
        private SchematicObject obj = null;
        [YamlIgnore]
        public override byte ClipSize { get; set; } = 1;
        public override ItemType Type { get; set; } = ItemType.GunCrossvec;

        public override AttachmentName[] Attachments { get; set; } = new[]
        {
            AttachmentName.NoRifleStock,
        };
        private Dictionary<int, float> shieldCooldowns = new();
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 4,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside096,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside106Primary,
                    Chance = 25
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidChamber,
                    Chance = 25
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideSurfaceNuke,
                    Chance = 12
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside330,
                    Chance = 13,
                }
            }
        };

        protected override void OnShot(ShotEventArgs ev)
        {
            if(!Check(ev.Player.CurrentItem))
                return;
            ev.Firearm.MagazineAmmo = 0;
            base.OnShot(ev);
        }

        private void OnAimDownSight(AimingDownSightEventArgs ev)
        {
            //if (Check(ev.Player.CurrentItem) && ev.AdsIn)
            //{
            //    obj = ObjectManager.SpawnObject("Shield", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, ev.Player.Transform.rotation);
            //    ObjectManager.RecolorAllPrimitives(obj, new Color(0.039f, 0.529f, 0.749f, 0.078f) * 4f);
            //}
            //else if (Check(ev.Player.CurrentItem) && ev.AdsIn == false)
            //{
            //    if (obj != null)
            //        ObjectManager.RemoveObject(obj);
            //}
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
                obj = ObjectManager.SpawnObject("Shield",
                    ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up,
                    ev.Player.Transform.rotation);

                ObjectManager.RecolorAllPrimitives(obj, new Color(0.039f, 0.529f, 0.749f, 0.078f) * 4f);
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