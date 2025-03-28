using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using YamlDotNet.Serialization;
using GhostPlugin.Methods.Objects;
using InventorySystem.Items.Firearms.Attachments;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Riveter : CustomWeapon
    {
        public override uint Id { get; set; } = 51;
        public override string Name { get; set; } = "Riveter";
        public override string Description { get; set; } = "AR 이지만 <color=#ebc934>12게이지 용의 숨결</color> 을 사용하는 연발 샷건입니다.";
        public override float Weight { get; set; } = 7.5f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz079,
                    Chance = 100,
                },
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczNuke,
                    Chance = 50,
                }
            }
        };
        public override byte ClipSize { get; set; } = 15;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        [YamlIgnore] 
        public override float Damage { get; set; }

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.LowcapMagJHP
        };
        
        private void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.IsAllowed = false;
            ev.Player.ShowHint("이 아이탬은 부착물 변경이 금지되어있습니다!", 3);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAttachments -= OnChangingAttachments;
            base.UnsubscribeEvents();
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                PlasmaCube dragonbreath = new PlasmaCube();
                SpawnParticleSpark spark = new SpawnParticleSpark();
                Color glowColor = new Color(1.0f, 0.0f, 0.0f, 0.1f) * 50f;
                spark.SpawnREDSpark(ev.Player,ev.Firearm.Base.transform.position,1);
                dragonbreath.SpawmSparkBuckshot(ev.Player, ev.Firearm.Base.transform.position,10,25f,0.1f,glowColor); 
            }
            base.OnShot(ev);
        }
    }
}