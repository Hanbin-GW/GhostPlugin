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
        public override string Description { get; set; } = "사용시 <b><color=#02dbc6>이동속도가 8초간 10% 증가하고 15HP 를 회복</color></b>합니다.";
        public override float Weight { get; set; } = 1.0f;
        public override SpawnProperties SpawnProperties { get; set; }
        
        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                ev.Player.EnableEffect<MovementBoost>(10,duration:8f);
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