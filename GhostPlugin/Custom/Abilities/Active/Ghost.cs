using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Ghost : ActiveAbility
    {
        public override string Name { get; set; } = "Ghost";
        public override string Description { get; set; } = "유령: 20초간 조용해지고 문을 통과하고 투명해집니다!";
        public override float Duration { get; set; } = 20;
        public override float Cooldown { get; set; } = 120;

        protected override void AbilityUsed(Player player)
        {
            player.EnableEffect<Ghostly>(duration: Duration);
            player.EnableEffect<Invisible>(duration:Duration);
            player.EnableEffect<SilentWalk>(duration: Duration);
            base.AbilityUsed(player);
        }

        protected override void AbilityEnded(Player player)
        {
            player.DisableEffect<Ghostly>();
            player.DisableEffect<SilentWalk>();
            base.AbilityRemoved(player);
        }
    }
}