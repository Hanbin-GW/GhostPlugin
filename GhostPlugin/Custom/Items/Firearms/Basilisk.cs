using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunRevolver)]
    public class Basilisk : CustomWeapon
    {
        public override uint Id { get; set; } = 44;
        public override string Name { get; set; } = "<color=red>Basilisk</color>";
        public override string Description { get; set; } = "대미지가 매우 높은 리볼버 입니다.";
        public override float Weight { get; set; } = 4.5f;
        public override ItemType Type { get; set; } = ItemType.GunRevolver;
        public override float Damage { get; set; } = 95;
        public override byte ClipSize { get; set; } = 6;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 10,
                    Location = SpawnLocationType.InsideLczArmory,
                },
                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new()
                {
                    Chance = 35,
                    Location = SpawnLocationType.Inside049Armory,
                },

                new()
                {
                    Chance = 40,
                    Location = SpawnLocationType.Inside079Armory,
                },
            }
        };

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.CylinderMag5,
            AttachmentName.ExtendedBarrel,
            AttachmentName.Flashlight,
        };
        private void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.IsAllowed = false;
            ev.Player.ShowHint("이 아이탬은 부착물 변경이 금지되어있습니다!", 3);
        }
        protected override void OnShot(ShotEventArgs ev)
        {
            /*var recoil = new RecoilSettings()
            {
                FovKick = 50f,
                UpKick = 60f,
                SideKick = 56f,
                ZAxis = 30f,
            };
            ev.Firearm.Recoil = recoil;
            ev.Firearm.DamageFalloffDistance = 80;
            SpawnParticleSpark spawnParticleSpark = new SpawnParticleSpark();
            spawnParticleSpark.SpawnREDSpark(ev.Player,ev.Firearm.Base.transform.position);*/
            base.OnShot(ev);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.ReloadingWeapon += OnReloading;
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.ReloadingWeapon -= OnReloading;
            Exiled.Events.Handlers.Item.ChangingAttachments -= OnChangingAttachments;
            base.UnsubscribeEvents();
        }

        /*protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (!(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "kr"))
            {
                player.ShowHint($"<size=32>You got a CustomItem {Name}</size>\n<size=28>The Revolver Which have a Big Damage</size>");
            }
            base.OnAcquired(player, item, displayMessage);
        }*/
    }
}