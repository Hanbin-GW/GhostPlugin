using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Attributes;
using Exiled.API.Structs;
using Exiled.CustomItems.API.Features;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorHeavy)]
    public class JuggernautArmor : CustomArmor
    {
        public override uint Id { get; set; } = 70;
        public override string Name { get; set; } = "저거넛 아머";
        public override string Description { get; set; } = "아주 무거운 고강도 아머입니다";
        public override ItemType Type { get; set; } = ItemType.ArmorHeavy;
        public override float Weight { get; set; } = 20f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override int VestEfficacy { get; set; } = 95;
        public override int HelmetEfficacy { get; set; } = 80;
        public override float StaminaUseMultiplier { get; set; } = 2f;

        public override List<ArmorAmmoLimit> AmmoLimits { get; set; } = new List<ArmorAmmoLimit>()
        {
            new ArmorAmmoLimit(AmmoType.Nato9, 300),
            new ArmorAmmoLimit(AmmoType.Nato556, 250),
            new ArmorAmmoLimit(AmmoType.Nato762, 250),
        };
    }
}