using System.Collections.Generic;
using System.Linq;
using System;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;

namespace GhostPlugin.Commands
{
    //[CommandHandler(typeof(ClientCommandHandler))]
    public class Chat : ICommand
    {
        private readonly Dictionary<RoleTypeId, List<string>> _teamMessages = new();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            string message = $"[{player.Nickname} : {string.Join(" ", arguments)}]";

            if (string.IsNullOrEmpty(string.Join("", arguments)))
            {
                response = "<color=red>메시지를 입력해야 합니다.</color>";
                return false;
            }

            var team = player.Role.Type;

            // 팀별 채팅 기록이 없으면 생성
            if (!_teamMessages.ContainsKey(team))
                _teamMessages[team] = new List<string>();

            _teamMessages[team].Add($"채팅 : {message}");

            // 최근 5개 메시지를 가져오기 (TakeLast 대체)
            List<string> lastMessages = _teamMessages[team].Skip(Math.Max(0, _teamMessages[team].Count - 5)).ToList();
            string broadcastMessage = string.Join("\n", lastMessages);

            // 같은 팀 플레이어에게 갱신된 메시지 브로드캐스트
            foreach (var p in Player.List.Where(p => p.Role.Type == team))
            {
                p.ClearBroadcasts(); // 기존 메시지 삭제
                p.Broadcast(5, $"<size=30><align=right><color=#bcff57>{broadcastMessage}</color></align></size>", 
                    global::Broadcast.BroadcastFlags.Normal,true);
            }

            response = $"메시지가 팀원들에게 전송되었습니다.\n{message}";

            return true;
        }

        public string Command { get; } = "Chat";
        public string[] Aliases { get; } = new[] { "c","chat","ㅊ" };
        public string Description { get; } = "팀원간의 채팅";
    }
}