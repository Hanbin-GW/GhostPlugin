using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using ProjectMER.Events.Arguments;
using ProjectMER.Events.Handlers;
using GhostPlugin.Methods.MER;
using MEC;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Coin)]
    public class AmmoBox : CustomItem
    {
        public override uint Id { get; set; } = 18;
        public override string Name { get; set; } = "Ammobox";
        public override string Description { get; set; } = "[T] 키를 눌르시 군수품 상자를 소환합니다.";
        public override float Weight { get; set; } = 4f;
        public override ItemType Type { get; set; } = ItemType.Coin;
        public override SpawnProperties SpawnProperties { get; set; }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDrop;
            Schematic.ButtonInteracted += OnButtonInteracted;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDrop;
            Schematic.ButtonInteracted -= OnButtonInteracted;
            base.UnsubscribeEvents();
        }

        private void OnDrop(DroppingItemEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (ev.IsThrown)
                {
                    ev.Item.Destroy();
                    SchematicObject Obj = ObjectManager.SpawnObject("Ammobox", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, Vector3.zero);
                    Timing.CallDelayed(60, () => ObjectManager.RemoveObject(Obj));
                }
            }
        }
        
        private void OnButtonInteracted(ButtonInteractedEventArgs ev)
        {
            if (ev.Schematic.name == "Ammobox") // 상호작용할 스키매틱 이름
            {
                Player player = ev.Player;
                // 탄약 지급
                player.AddAmmo(AmmoType.Nato556,40);
                player.ShowHint("<color=yellow>5.56mm 탄약 40개를 받았습니다!</color>", 3f);
            }
        }
    }
}