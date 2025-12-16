using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.MicroHID)]
    public class LaserCannon: CustomItem, ICustomItemGlow
    {
        public override uint Id { get; set; } = 41;
        public override string Name { get; set; } = "<color=#ffd900>Laser cannon</color>";
        public override string Description { get; set; } = "This is an unlimited rail gun.";
        public override float Weight { get; set; } = 30f;
        public override ItemType Type { get; set; } = ItemType.MicroHID;
        public override SpawnProperties SpawnProperties { get; set; }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingMicroHID;
            //Exiled.Events.Handlers.Player.ChangingItem += OnEquipMicroHid;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= OnUsingMicroHID;
            //Exiled.Events.Handlers.Player.ChangingItem -= OnEquipMicroHid;
            base.UnsubscribeEvents();
        }

        private void OnUsingMicroHID(UsingMicroHIDEnergyEventArgs ev)
        {
            if (Check(ev.Item.Owner.CurrentItem))
            {
                ev.MicroHID.Energy = 100f;
            }
        }

        private void OnChangingMicroHIDState(ChangingMicroHIDStateEventArgs ev)
        {
            
        }

        private void OnChargingMircoHID(ChangingMicroHIDStateEventArgs ev)
        {
            if (Check(ev.Item.Owner.CurrentItem))
            {
                //ev.MicroHID.Energy += 1;
            }
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(255,255,0,127);
        public float GlowRange { get; set; } = 0.15f;

    }
}