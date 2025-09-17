using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Focus : ActiveAbility
    {
        public override string Name { get; set; } = "Focus";
        public override string Description { get; set; } = "Get a SCP1853 Effect for 20 secounds.";
        public override float Duration { get; set; } = 20;
        public override float Cooldown { get; set; } = 100;

        protected override void AbilityUsed(Player player)
        {
            player.EnableEffect<Scp1853>(intensity:4,Duration);
            base.AbilityUsed(player);
        }

        protected override void AbilityEnded(Player player)
        {
            player.DisableEffect<Scp1853>();
            base.AbilityRemoved(player);
        }
    }
}