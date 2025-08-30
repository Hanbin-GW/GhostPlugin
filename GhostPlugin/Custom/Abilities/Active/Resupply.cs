using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    public class Resupply : ActiveAbility
    {
        public override string Name { get; set; } = "Resupply";
        public override string Description { get; set; } = "Resupply HE & Flash Gernade !";
        public override float Duration { get; set; } = 1f;
        public override float Cooldown { get; set; } = 60f;
        
        protected override void AbilityUsed(Player player)
        {
            player.AddItem(ItemType.GrenadeHE);
            player.AddItem(ItemType.GrenadeFlash);
        }
    }
}