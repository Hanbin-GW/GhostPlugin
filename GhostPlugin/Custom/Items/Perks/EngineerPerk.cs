using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Abilities.Active;

namespace GhostPlugin.Custom.Items.Perks
{
    public class EngineerPerk : CustomItem
    {
        public override uint Id { get; set; } = 65;
        public override string Name { get; set; } = "<color=#68d9e3>Engineer Perk</color>";
        public override string Description { get; set; } = "이 퍽을 적용시키고 작동시키면 문을 강제로 열을수 있습니다.";
        public override float Weight { get; set; } = 1.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                Plugin.Instance.PerkEventHandlers.GrantAbility(ev.Player, new DoorPicking());
                ev.Item.Destroy();
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            base.UnsubscribeEvents();
        }
    }
}