using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Abilities.Passive
{
    [CustomAbility]
    public class UltraBoostOnKill : PassiveAbility
    {
        public override string Name { get; set; } = "Boost On Kill\n[Legend Edition]";

        public override string Description { get; set; } =
            "Each time you kill an enemy, your speed increases by 10% per treatment per +5 seconds.";
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents(); 
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }
        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.Attacker.EnableEffect<MovementBoost>(intensity: +10, duration: +5);
            }
        }
    }
}