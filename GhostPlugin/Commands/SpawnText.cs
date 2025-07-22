using System;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods.ToyUtils;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnText : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (player == null)
            {
                response = "플레이어를 찾을 수 없습니다.";
                return false;
            }
            TextUtils.SpawnText(player, player.Position, "Content Restored", 60);
            response = "Spawned";
            return true;
        }

        public string Command { get; } = "SpawnText";
        public string[] Aliases { get; } = new[] { "ST" };
        public string Description { get; } = "ToyUtils 소환";
    }
}