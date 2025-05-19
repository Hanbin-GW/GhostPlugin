using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorCombat)]
    public class AssassinVest : CustomArmor
    {
        public override uint Id { get; set; } = 19;
        public override string Name { get; set; } = "<color=#a1a1a1>Assassin Vest</color>";
        public override string Description { get; set; } = "SCP-939 does not detect sound when using this armor.";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.ArmorCombat;

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            player.EnableEffect<SilentWalk>();
            base.OnAcquired(player, item, displayMessage);
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            ev.Player.DisableEffect<SilentWalk>();
            base.OnDroppingItem(ev);
        }
    }
}