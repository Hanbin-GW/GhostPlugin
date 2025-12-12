using Exiled.API.Features.Pickups;
using GhostPlugin.API.CustomItem;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace GhostPlugin.Patches
{
    internal static class StickyGrenadeCollision
    {
        static bool Prefix(CollisionDetectionPickup __instance, Collision collision)
        {
            Pickup pickup = Pickup.Get(__instance.gameObject);
            if (pickup == null)
                return true;

            if (StickyGrenadeApi.IsStickyGrenade(pickup) && !StickyGrenadeApi.CollidedGrenades.ContainsKey(pickup))
            {
                StickyGrenadeApi.CollidedGrenades[pickup] = true;
            }
            
            return true;
        }
    }
}