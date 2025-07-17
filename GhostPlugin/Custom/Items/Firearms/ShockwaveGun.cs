using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.Objects;
using InventorySystem.Items.Firearms.Attachments;
using PlayerRoles;
using PlayerStatsSystem;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCrossvec)]
    public class ShockwaveGun : CustomWeapon
    {
        public override uint Id { get; set; } = 4;
        public override string Name { get; set; } = "Shockwave Electricity Gun";
        public override string Description { get; set; } = "A prototype machine gun with special ammunition.\nVery critical to Scp049-2.";
        public override float Weight { get; set; } = 5;
        public override ItemType Type { get; set; } = ItemType.GunCrossvec;
        [YamlIgnore]
        public override float Damage { get; set; }
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 60;
        public float HumanDamageMultiplier { get; set; } = 1.2f;
        public float ZombieDamageMultiplier { get; set; } = 2.1f;
        public float ScpDamageMultiplier { get; set; } = 1.5f;

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.ExtendedBarrel,
            AttachmentName.Laser,
        };

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem) && ev.Player.IsHuman)
                ev.Amount *= HumanDamageMultiplier;
            if (Check(ev.Player.CurrentItem) && ev.Player.Role.Type is RoleTypeId.Scp0492)
                ev.Amount *= ZombieDamageMultiplier;
            if (Check(ev.Player.CurrentItem) && ev.Player.IsScp)
                ev.Amount *= ScpDamageMultiplier;

        }
        
        protected override void OnShot(ShotEventArgs ev)
        {
            /*SpawnLaserObject spawnLaserObject = new SpawnLaserObject();
            spawnLaserObject.SpawnLaser(ev.Player);*/
            base.OnShot(ev);
        }
    }
}