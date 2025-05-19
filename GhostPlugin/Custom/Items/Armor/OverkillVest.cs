using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;

namespace GhostPlugin.Custom.Items.Armor
{
    public class OverkillVest : CustomArmor
    {
        public override uint Id { get; set; } = 45;
        public override string Name { get; set; } = "Overkill Vest";
        public override string Description { get; set; } = "[Alt] 를 눌러 렌덤으로 총기 1개를 획득합니다!";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.ArmorCombat;

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            base.OnAcquired(player, item, displayMessage);
        }
    }
}