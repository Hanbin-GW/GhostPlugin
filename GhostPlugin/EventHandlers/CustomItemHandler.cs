using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;

namespace GhostPlugin.EventHandlers
{
    public class CustomItemHandler
    {
        public void OnInspectingItem(InspectingItemEventArgs ev)
        {
            if (CustomItem.TryGet(ev.Item, out CustomItem customItem))
            {
                if(customItem != null)
                    ev.Player.ShowHint($"<b><color=yellow>{customItem.Name}</color></b>\n<size=20>{customItem.Description}</size>", 5f);
            }
        }
    }
}