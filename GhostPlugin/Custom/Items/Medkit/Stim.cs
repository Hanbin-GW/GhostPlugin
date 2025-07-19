using CustomPlayerEffects;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Items.Medkit
{
    public class Stim : CustomItem
    {
        public override uint Id { get; set; } = 13;
        public override string Name { get; set; } = "<color=#dfff61>전투 자극제</color>";
        public override string Description { get; set; } = "<b><color=#02dbc6>In use, increase the movement speed by 10% for 8 seconds and recover 15HP</color></b>.";
        public override float Weight { get; set; } = 1.0f;
        public override SpawnProperties SpawnProperties { get; set; }
        
        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                ev.Player.EnableEffect<MovementBoost>(20,duration:8f);
                ev.Player.Heal(20);
            }
        }

        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }
    }
}