using System;
using Exiled.API.Features;
using CommandSystem;
using Exiled.CustomRoles.API.Features;
using RemoteAdmin;


namespace GhostPlugin.Commands{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RoleInfo : ICommand
    {
        public string Command { get; } = "RoleInfo";
        public string[] Aliases { get; } = new[] {"ri"};
        public string Description { get; } = "Check a Role Description";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                Player player = Player.Get(playerSender.ReferenceHub);
                
                if (player != null)
                {
                    foreach (CustomRole customRole in CustomRole.Registered)
                    {
                        if (customRole.Check(player))
                        {
                            player.ShowHint($"Role Name: {customRole.Name}\nDescription: {customRole.Description}\nRole: {customRole.Role}");
                            player.Broadcast(5,$"Role Name: {customRole.Name}\nDescription: {customRole.Description}\nRole: {customRole.Role}");
                            response = $"You are {customRole.Name}.\n{customRole.Description}";
                            return true;
                        }
                    }
                    response = "<color=red>You don't have any CustomRole.</color>";
                    return false;
                }
            }

            response = "플래이어가 아닙니다. 또는 커스텀 역할이 없습니다.";
            return false;
        }
    }
}
