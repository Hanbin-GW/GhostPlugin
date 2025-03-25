/*using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using HintServiceMeow.UI.Utilities;
using MEC;
using UnityEngine;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatRueI : ICommand
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

            if (!_teamMessages.ContainsKey(team))
                _teamMessages[team] = new List<string>();

            _teamMessages[team].Add($"채팅 : {message}");

            List<string> lastMessages = _teamMessages[team].Skip(Math.Max(0, _teamMessages[team].Count - 5)).ToList();

            // RueI 힌트 스타일링
            
            int yOffset = 200;
            foreach (string msg in lastMessages)
            {
                Hint hint = new Hint
                {
                    Text = $"<size=30><color=#bcff57>{msg}</color></size>",
                    YCoordinate = yOffset,
                    Alignment = HintAlignment.Right,
                    Id = Guid.NewGuid().ToString() // 고유 ID 생성
                };

                foreach (var p in Player.List.Where(p => p.Role.Type == team))
                {
                    PlayerDisplay display = PlayerDisplay.Get(p);
                    display.AddHint(hint);

                    // 개별 힌트 삭제
                    Timing.CallDelayed(7, () => display.RemoveHint(hint));
                }
                yOffset -= 30;
            }
            // 실제 화면에 표시
            

            response = $"메시지가 팀원들에게 전송되었습니다.\n{message}";
            return true;
        }

        public string Command { get; } = "ChatRueI";
        public string[] Aliases { get; } = new[] { "cr", "chat_r" };
        public string Description { get; } = "팀원간의 채팅 (RueI 힌트)";
    }
}
*/