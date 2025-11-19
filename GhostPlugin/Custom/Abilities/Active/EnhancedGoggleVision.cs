using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class EnhancedGoggleVision : ActiveAbility
    {
        public override string Name { get; set; } = "Enhanced Goggle Vision";
        public override string Description { get; set; } = "You can look beyond the enemy's wall for 30 seconds.";
        public override float Duration { get; set; } = 30;
        public override float Cooldown { get; set; } = 60;

        protected override void AbilityUsed(Player player)
        {
            player.EnableEffect<Scp1344>(intensity:4);
            base.AbilityUsed(player);
        }
        protected override void AbilityEnded(Player player)
        {
            player.DisableEffect<Scp1344>();
            base.AbilityRemoved(player);
        }
    }
}