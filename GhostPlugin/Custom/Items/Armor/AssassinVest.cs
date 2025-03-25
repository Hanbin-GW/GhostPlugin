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
        public override string Name { get; set; } = "<color=#a1a1a1>암살자 조끼</color>";
        public override string Description { get; set; } = "이 아머를 사용할시 SCP-939 가 소리를 감지하지 못합니다.";
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