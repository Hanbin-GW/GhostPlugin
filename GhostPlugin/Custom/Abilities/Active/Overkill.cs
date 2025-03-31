using System;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using InventorySystem;
using InventorySystem.Items;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Overkill : ActiveAbility
    {
        public override string Name { get; set; } = "OverKill";
        public override string Description { get; set; } = "렌덤으로 무기를 얻습니다!";
        public override float Duration { get; set; } = 1f;
        public override float Cooldown { get; set; } = 120;

        protected override void AbilityUsed(Player player)
        {
            GiveRandomWeapon(player);
            base.AbilityUsed(player);
        }

        private void GiveRandomWeapon(Player player)
        {
            ItemType[] weaponTypes = Enum.GetValues(typeof(ItemType))
                .Cast<ItemType>()
                .Where(x => x.IsWeapon())
                .ToArray();
            ItemType randomWeapon = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Length)];
            ItemBase newItem = player.Inventory.ServerAddItem(randomWeapon, ItemAddReason.AdminCommand);
            ushort serial = newItem.ItemSerial;
            player.Inventory.ServerSelectItem(serial);
            //player.AddItem(randomWeapon);
            player.ShowHint($"오버킬 능력으로 {randomWeapon} 을 받았습니다!", 5);

        }
    }
}