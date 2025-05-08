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
        public int AllowedNumber = 3;
        private int usednumber = 0;
        public SchematicObject obj = null;

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
                    obj = ObjectManager.SpawnObject("Ammobox", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, Vector3.zero);
                    Timing.CallDelayed(60, () => ObjectManager.RemoveObject(obj));
                }
            }
        }
        
        private void OnButtonInteracted(ButtonInteractedEventArgs ev)
        {
            if (ev.Schematic?.Name == "Ammobox") // 상호작용할 스키매틱 이름
            {
                usednumber++;
                if (usednumber > AllowedNumber)
                {
                    ObjectManager.RemoveObject(obj);
                }
                Player player = ev.Player;
                player.AddAmmo(AmmoType.Nato556,80);
                player.AddAmmo(AmmoType.Nato9, 90);
                player.AddAmmo(AmmoType.Ammo44Cal, 18);
                player.AddAmmo(AmmoType.Ammo12Gauge, 10);
                player.AddAmmo(AmmoType.Nato762, 60);
                player.ShowHint("<color=yellow>탄약 제공 완료 받았습니다!</color>", 3f);
            }
        }
    }
}