using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.Custom.Items.Grenades;

namespace GhostPlugin.Custom.Abilities.Active
{
    public class Shockwave : ActiveAbility
    {
        public override string Name { get; set; } = "충격파";
        public override string Description { get; set; } = "능력을 사용하여 충격파로 적들을 일시적으로 마비시킵니다!";
        public override float Duration { get; set; } = 1f;
        public override float Cooldown { get; set; } = 35f;

        protected override void AbilityUsed(Player player)
        {
            var grenade = new StunGrenade();
            var pickup = grenade.Throw(
                position: player.Position,
                force: 0f,
                weight:0.1f,
                grenadeType: grenade.Type,
                fuseTime: 0.3f,
                player: player
             );
            player.DisableEffect<Slowness>();
            base.AbilityUsed(player);
        }
    }
}