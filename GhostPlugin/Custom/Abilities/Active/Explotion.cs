using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.Custom.Roles.Scps;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Explotion : ActiveAbility
    {
        public override string Name { get; set; } = "SCP-457 Explosion";
        public override string Description { get; set; } = "일시적으로 둔해지는대신 거의무적이 되면서 자폭합니다\n049 - Soul Stealer 의 전용 능력입니다!";
        public override float Duration { get; set; } = 6f;
        public override float Cooldown { get; set; } = 70f;

        protected override void AbilityUsed(Player player)
        {
            base.AbilityUsed(player);
            player.IsGodModeEnabled = true;
            Vector3 position = player.Position;
            ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(position, owner:player);
            player.EnableEffect<Slowness>(intensity: 90, duration: 6);
            base.AbilityUsed(player);
        }
        protected override void AbilityEnded(Player player)
        {
            player.IsGodModeEnabled = false;
            player.DisableEffect<Slowness>();
            base.AbilityRemoved(player);
        }
    }
}