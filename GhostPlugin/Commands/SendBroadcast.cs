using System;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods.Administration;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SendBroadcast : ICommand
    {
        /// <summary>
        /// 모든 플레이어에게 텍스트, 색상, 크기를 설정하여 브로드캐스트 메시지를 보냅니다.
        /// 사용법: SendBroadcast &lt;메시지&gt; &lt;색깔&gt; &lt;텍스트 크기&gt;
        /// </summary>
        /// <param name="arguments">메시지, 색깔, 텍스트 크기를 포함한 명령어 인자입니다.</param>
        /// <param name="sender">명령어를 실행한 사람입니다. 보통 RemoteAdmin 관리자입니다.</param>
        /// <param name="response">명령어 실행 결과로 반환되는 메시지입니다.</param>
        /// <returns>성공적으로 메시지를 보냈다면 true, 실패했다면 false를 반환합니다.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            string message = String.Empty;
            string message_Argumanet = arguments.At(0).ToLower();
            string message_Color = arguments.At(1).ToLower();
            string message_Size = arguments.At(2).ToLower();
            string duration = arguments.At(3).ToLower();
            if (player == null)
            {
                response = "플레이어를 찾을 수 없습니다.";
                return false;
            }

            if (Plugin.Instance.Config.Debug)
            {
                SendBroadcastGhost.BroadcastGhost.BroadcastToAll(message_Argumanet, message_Color,
                    message_Size, ushort.Parse(duration));
            }

            if (arguments.Count < 3)
            {
                response = "사용법: SendBroadcast <메시지> <색깔> <택스트 사이즈>";
                return false;
            }

            foreach (Player players in Player.List)
            {
                players.Broadcast(5,message);
            }
            message = $"<size={message_Size}><color={message_Color}>{message_Argumanet}</color></size>";
            response = $"메시지가 모든 유저한테 전송되었습니다:      {message}";
            return true;
        }
        public string Command { get; } = "SendBroadcast";
        public string[] Aliases { get; } = new[] { "sb" };
        public string Description { get; } = "Send Broadcast To user";
    }
}