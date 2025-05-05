using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Abilities.Passive
{
    [CustomAbility]
    public class BoostOnKill : PassiveAbility
    {
        public override string Name { get; set; } = "Boost On Kill";
        public override string Description { get; set; } = "적을 처치할 때마다 이동속도가 증가합니다";
        
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
                ev.Attacker.EnableEffect<MovementBoost>(intensity: 15, duration: +5);
            }
        }
    }
}