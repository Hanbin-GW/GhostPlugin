using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class SCP106 : ActiveAbility
    {
        public override string Name { get; set; } = "SCP106";
        public override string Description { get; set; } = "106 의 능력을 사용할수 있습니다!\n049 - Soul Stealer 의 전용 능력입니다!";
        public override float Duration { get; set; } = 20;
        public override float Cooldown { get; set; } = 120;

        protected override void AbilityUsed(Player player)
        {
            player.EnableEffect<Ghostly>(duration: Duration);
            player.EnableEffect<DamageReduction>(duration: Duration, intensity:50);
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