using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items;

namespace GhostPlugin.Utilities
{
    public static class CustomItemHelper
    {
        /// <summary>
        /// Verify that the specified Inventory System item has a specific CustomItem ID.
        /// </summary>
        public static bool IsCustomItem(this ItemBase itemBase, uint customId)
        {
            if (itemBase == null)
                return false;

            var exiledItem = Item.Get(itemBase);
            if (exiledItem == null)
                return false;

            if (!CustomItem.TryGet(exiledItem, out var custom))
                return false;

            return custom.Id == customId;
        }

        /// <summary>
        /// Verify that the Exiled Item object has a specific Custom Item ID.
        /// </summary>
        public static bool IsCustomItem(this Item item, uint customId)
        {
            if (item == null) return false;
            return CustomItem.TryGet(item, out var c) && c.Id == customId;
        }
    }
}