using Exiled.Events.Features;
using Exiled.Events.EventArgs;
using System.Globalization;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.EventHandlers
{
    public class CustomItemHandler
    {
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem; 
        }

        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        }
        
        private static void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            string language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}