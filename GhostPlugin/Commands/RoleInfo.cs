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
                        if (customRole.Check(player)) // 플레이어가 해당 커스텀 롤을 가지고 있는지 확인
                        {
                            player.ShowHint($"Role Name: {customRole.Name}\nDescription: {customRole.Description}\nRole: {customRole.Role}");
                            player.Broadcast(5,$"Role Name: {customRole.Name}\nDescription: {customRole.Description}\nRole: {customRole.Role}");
                            response = $"당신은 {customRole.Name}입니다.\n{customRole.Description} \n\n커스텀 역할 정보가 성공적으로 출력되었습니다.";
                            return true;
                        }
                    }
                    response = "<color=red>커스텀 롤이 없습니다.</color>";
                    return false;
                }
            }

            response = "플래이어가 아닙니다. 또는 커스텀 역할이 없습니다.";
            return false;
        }
    }
}
