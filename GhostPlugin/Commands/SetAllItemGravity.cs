using System;
using CommandSystem;
using Exiled.API.Features.Pickups;
using UnityEngine;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetAllItemGravity : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Vector3 gravity = new Vector3(-1f, -3f, 0f);
            foreach (Pickup pickup in Pickup.List)
            {
                if (pickup != null)
                {
                    pickup.GameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody);
                    if (pickup.Rigidbody != null)
                    {
                        pickup.Rigidbody.useGravity = false;
                        if (pickup.GameObject.TryGetComponent<ConstantForce>(out ConstantForce constantForce))
                        {
                            constantForce.force = gravity;
                        }
                        else
                        {
                            pickup.GameObject.AddComponent<ConstantForce>().force = gravity;
                        }
                    }
                }
            }
            response = "Every PickupGravity Is <color=red>Changed</color> :3";
            return true;
        }

        public string Command { get; } = "SetAllItemGravity";
        public string[] Aliases { get; } = new[] { "saig" };
        public string Description { get; } = "Set All ItemGravity to false";
    }
}