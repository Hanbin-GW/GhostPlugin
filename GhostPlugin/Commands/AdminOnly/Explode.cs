using System;
using CommandSystem;
using Exiled.API.Features;

namespace GhostPlugin.Commands.AdminOnly
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Explode : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "use: .explode <playerId>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int id))
            {
                response = "Insert the player ID.";
                return false;
            }

            Player target = Player.Get(id);

            if (target == null)
            {
                response = "Unable to find player ID.";
                return false;
            }
            
            target.Explode();
            
            response = $"You explode {target.Nickname}!";
            return true;
        }

        public string Command { get; } = "Explode";
        public string[] Aliases { get; } = new[] { "e", "explode" };
        public string Description { get; } = "Explode the player, <color=red>Warning use wisely</color>\nUseage: .eplode <player id>";
    }
}