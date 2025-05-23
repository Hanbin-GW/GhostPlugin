using System;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using InventorySystem;
using InventorySystem.Items;

namespace GhostPlugin.Custom.Abilities.Active
{
    public class Overkill : ActiveAbility
    {
        public override string Name { get; set; } = "Overkill";
        public override string Description { get; set; } = "사용시 랜덤으로 무기 하나를 확득합니다!";
        public override float Duration { get; set; } = 1f;
        public override float Cooldown { get; set; } = 45f;

        protected override void AbilityUsed(Player player)
        {
            base.AbilityUsed(player);
            GiveRandomWeapon(player);
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