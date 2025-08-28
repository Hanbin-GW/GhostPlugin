using System.Collections.Generic;
using System.Linq;
using System;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Chat : ICommand
    {
        private readonly Dictionary<RoleTypeId, List<string>> _teamMessages = new();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            string message = $"[{player.Nickname} : {string.Join(" ", arguments)}]";

            if (string.IsNullOrEmpty(string.Join("", arguments)))
            {
                response = "<color=red>You should write the message.</color>";
                return false;
            }

            var team = player.Role.Type;

            // Create if you don't have a chat history for each team
            if (!_teamMessages.ContainsKey(team))
                _teamMessages[team] = new List<string>();

            _teamMessages[team].Add($"chat : {message}");

            // Import the last 5 messages (replace TakeLast)
            List<string> lastMessages = _teamMessages[team].Skip(Math.Max(0, _teamMessages[team].Count - 5)).ToList();
            string broadcastMessage = string.Join("\n", lastMessages);

            // Broadcast updated messages to the same team player
            foreach (var p in Player.List.Where(p => p.Role.Type == team))
            {
                p.ClearBroadcasts(); // Delete existing messages
                p.Broadcast(5, $"<size=30><align=right><color=#bcff57>{broadcastMessage}</color></align></size>", 
                    global::Broadcast.BroadcastFlags.Normal,true);
            }

            response = $"Your message has been sent to your team members.\n{message}";

            return true;
        }

        public string Command { get; } = "Chat";
        public string[] Aliases { get; } = new[] { "c","chat","ㅊ" };
        public string Description { get; } = "Chat with your team mates!";
    }
}