using System;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods.ToyUtils;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Test : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            var light = LightUtils.SpawnLight(player,player.Position + Vector3.up, Quaternion.identity, 4f, 12f, Color.green);
            response = "light spawned";
            Timing.CallDelayed(10, () => Object.Destroy(light));
            return true;
        }

        public string Command { get; } = "test";
        public string[] Aliases { get; } = new[] { "t" };
        public string Description { get; } = "test light command";
    }
}