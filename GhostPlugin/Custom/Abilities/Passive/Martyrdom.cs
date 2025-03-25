using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Item = Exiled.API.Features.Items.Item;
using Player = Exiled.Events.Handlers.Player;

namespace GhostPlugin.Custom.Abilities.Passive
{
    [CustomAbility]
    public class Martyrdom : PassiveAbility
    {
        public override string Name { get; set; } = "Martyrdom";
        public override string Description { get; set; } = "Causes the player to explode upon death.";
        public float ExplosiveFuse { get; set; } = 1f;

        protected override void AbilityAdded(Exiled.API.Features.Player player1)
        {
            Player.Dying += OnDying;
            base.AbilityAdded(player1);
        }

        protected override void AbilityRemoved(Exiled.API.Features.Player player1)
        {
            Player.Dying -= OnDying;
            base.AbilityRemoved(player1);
        }
        
        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                Log.Debug($"VVUP Custom Abilities: Spawning Grenade at {ev.Player.Nickname} death location");
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = ExplosiveFuse;
                grenade.ChangeItemOwner(Server.Host, ev.Player);
                grenade.SpawnActive(ev.Player.Position);
            }
        }
    }
}