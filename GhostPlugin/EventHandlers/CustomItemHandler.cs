using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Map;
using GhostPlugin.API;
using Mirror;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace GhostPlugin.EventHandlers
{
    public class CustomItemHandler
    {
        public Plugin Plugin;
        public CustomItemHandler(Plugin plugin) => Plugin = plugin;
        private static readonly Dictionary<Pickup, Light> ActiveGlowEffects = new Dictionary<Pickup, Light>();

        public void OnInspectingItem(InspectingItemEventArgs ev)
        {
            if (CustomItem.TryGet(ev.Item, out CustomItem customItem))
            {
                if(customItem != null)
                    ev.Player.ShowHint(new string('\n', 10) + $"<b><color=yellow>{customItem.Name}</color></b>\n<size=20>{customItem.Description}</size>", 5f);
            }
        }
        public void OnRoundStarted()
        {
            foreach (Pickup pickup in Pickup.List)
            {
                CustomItem.TryGet(pickup, out CustomItem ci);
                if (ci is ICustomItemGlow { HasCustomItemGlow: true } glowableItem)
                {
                    ApplyGlowEffect(pickup, glowableItem.CustomItemGlowColor);
                }
            }
        }
        public void AddGlow(PickupAddedEventArgs ev)
        {
            CustomItem.TryGet(ev.Pickup, out CustomItem ci);
            if (ci is ICustomItemGlow { HasCustomItemGlow: true } glowableItem)
            {
                ApplyGlowEffect(ev.Pickup, glowableItem.CustomItemGlowColor);
            }
        }
        
        public void RemoveGlow(PickupDestroyedEventArgs ev)
        {
            if (ev.Pickup == null || ev.Pickup?.Base?.gameObject == null)
                return;
            if (!ActiveGlowEffects.ContainsKey(ev.Pickup)) 
                return;
            if (CustomItem.TryGet(ev.Pickup.Serial, out CustomItem ci) && ci is ICustomItemGlow { HasCustomItemGlow: true })
            {
                RemoveGlowEffect(ev.Pickup);
            }
        }
        
        public void OnWaitingForPlayers()
        {
            ClearAllGlowEffects();
        }

        private void ApplyGlowEffect(Pickup pickup, Color glowColor, float range = 0.25f)
        {
            if (ActiveGlowEffects.ContainsKey(pickup))
            {
                RemoveGlowEffect(pickup);
            }
            
            var light = Light.Create(pickup.Position);
            light.Color = glowColor;
            light.Range = range;
            light.ShadowType = LightShadows.Soft;
            light.Base.gameObject.transform.SetParent(pickup.Base.gameObject.transform);
            ActiveGlowEffects[pickup] = light;
        }


        private void RemoveGlowEffect(Pickup pickup)
        {
            var light = ActiveGlowEffects[pickup];
            if (light != null && light.Base != null)
            {
                NetworkServer.Destroy(light.Base.gameObject);
            }
            ActiveGlowEffects.Remove(pickup);
        }

        private void ClearAllGlowEffects()
        {
            foreach (var light in ActiveGlowEffects.Select(lights => lights.Value)
                         .Where(light => light != null && light.Base != null))
            {
                try
                {
                    NetworkServer.Destroy(light.Base.gameObject);
                }
                catch
                {
                     // You know it would be extremely hilarious if I didn't do anything.
                }
            }
            ActiveGlowEffects.Clear();
        }
    }
}