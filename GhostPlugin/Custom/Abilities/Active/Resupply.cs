using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;

namespace GhostPlugin.Custom.Abilities.Active
{
    public class Resupply : ActiveAbility
    {
        public override string Name { get; set; } = "Resupply";
        public override string Description { get; set; } = "수류탄과 섬광탄을 재보충해줍니다!";
        public override float Duration { get; set; } = 1f;
        public override float Cooldown { get; set; } = 60f;
        
        protected override void AbilityUsed(Player player)
        {
            player.AddItem(ItemType.GrenadeHE);
            player.AddItem(ItemType.GrenadeFlash);
        }
    }
}