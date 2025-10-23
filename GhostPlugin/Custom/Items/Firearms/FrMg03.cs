using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunFRMG0)]
    public class FrMg03 : CustomWeapon
    {
        public override uint Id { get; set; } = 72;
        public override string Name { get; set; } = "FR MG 03";
        public override string Description { get; set; } = "Reinforced bersion of FR-MG-01\nIt takes a long time and uses 5.56 Low Grain Rounds, so the damage is a little low,\nThe ammunition capacity is 185 rounds.";
        public override float Weight { get; set; } = 18f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunFRMG0;
        public override float Damage { get; set; } = 24;
        public override byte ClipSize { get; set; } = 185;

        public override AttachmentName[] Attachments { get; set; } = new AttachmentName[]
        {
            AttachmentName.Foregrip,
            AttachmentName.HoloSight,
        };
        private void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.IsAllowed = false;
            ev.Player.ShowHint("이 아이탬은 부착물 변경이 금지되어있습니다!", 3);
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            base.OnReloading(ev);
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.Player.EnableEffect<Ensnared>(duration:2f);
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
    }
}