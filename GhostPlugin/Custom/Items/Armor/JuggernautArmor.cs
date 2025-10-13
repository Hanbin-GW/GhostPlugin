using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Structs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerStatsSystem;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorHeavy)]
    public class JuggernautArmor : CustomArmor
    {
        public override uint Id { get; set; } = 70;
        public override string Name { get; set; } = "저거넛 아머";
        public override string Description { get; set; } = "아주 무거운 고강도 아머입니다";
        public override ItemType Type { get; set; } = ItemType.ArmorHeavy;
        public override float Weight { get; set; } = 20f;
        public override SpawnProperties SpawnProperties { get; set; }
        //public override int VestEfficacy { get; set; } = 95;
        //public override int HelmetEfficacy { get; set; } = 80;
        //public override float StaminaUseMultiplier { get; set; } = 2f;

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor))
            {
                if (ev.Player.ArtificialHealth != 0)
                    ev.Amount = 0;
                else
                    ev.Amount *= 0.15f;
            }
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (!Check(player.CurrentArmor))
                return;
            var stamina = player.GetModule<StaminaStat>();
            stamina.ModifyAmount(0.4f);
            base.OnAcquired(player, item, displayMessage);
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;
            var stamina = ev.Player.GetModule<StaminaStat>();
            stamina.ModifyAmount(0.05f);
            base.OnDroppingItem(ev);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }
        /*public override List<ArmorAmmoLimit> AmmoLimits { get; set; } = new List<ArmorAmmoLimit>()
        {
            new ArmorAmmoLimit(AmmoType.Nato9, 300),
            new ArmorAmmoLimit(AmmoType.Nato556, 250),
            new ArmorAmmoLimit(AmmoType.Nato762, 250),
        };*/
    }
}