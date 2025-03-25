using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunAK)]
    public class Svd : CustomWeapon
    {
        public override float Damage { get; set; } = 80f;
        public override uint Id { get; set; } = 17;
        public override string Name { get; set; } = "<color=#006826>SVD</color>";
        public override string Description { get; set; } = "<color=#006826>AK 의 개조버전</color>이고 대미지는 80 입니다.";
        public override float Weight { get; set; } = 3.6f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunAK;

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.ExtendedBarrel,
            AttachmentName.ScopeSight,
            AttachmentName.HeavyStock,
            AttachmentName.Laser,
            AttachmentName.StandardMagAP,
        };
        public override byte ClipSize { get; set; } = 5;

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            base.OnReloading(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            ev.Firearm.Penetration = 150;
            ev.Firearm.DamageFalloffDistance = 150;
            base.OnShot(ev);
        }
    }
    
}