using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Custom.Abilities.Passive;

namespace GhostPlugin.Custom.Items.Perks
{
    public class OverkillPerk : CustomItem
    {
        public override uint Id { get; set; } = 66;
        public override string Name { get; set; } = "OverKill Perk";
        public override string Description { get; set; } = "동전을 돌릴시 오버킬 능력을 얻을수 있습니다!";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; }
        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                Plugin.Instance.PerkEventHandlers.GrantAbility(ev.Player, new Overkill());
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