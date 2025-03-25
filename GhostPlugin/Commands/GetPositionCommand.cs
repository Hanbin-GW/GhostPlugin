using RemoteAdmin;
using UnityEngine;
namespace GhostPlugin.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using System;

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GetPositionCommand : ICommand
    {
        public string Command { get; } = "getpos";
        public string Description { get; } = "Gets the current position of the player.";

        public string[] Aliases { get; } = { "pos", "position" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                Player player = Player.Get(playerSender.ReferenceHub);
                if (player != null)
                {
                    Vector3 position = player.Position;
                
                    response = $"Your current position is: {position}";
                    Log.Info(response);
                    return true;
                }
                else
                {
                    response = "Player not found.";
                    return false;
                }
            }
            response = "This command can only be used by a player.";
            return false;
        }
    }
}