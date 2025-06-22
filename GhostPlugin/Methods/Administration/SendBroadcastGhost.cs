using Exiled.API.Features;

namespace GhostPlugin.Methods.Administration
{
    public class SendBroadcastGhost
    {
        /// <summary>
        /// 모든 플레이어에게 지정된 텍스트, 색상, 크기, 지속시간을 가진 브로드캐스트 메시지를 전송합니다.
        /// </summary>
        /// <param name="message">전송할 브로드캐스트 메시지입니다.</param>
        /// <param name="color">텍스트 색상입니다. 예: "red", "#FF0000".</param>
        /// <param name="size">텍스트 크기입니다. 예: "20".</param>
        /// <param name="duration">브로드캐스트가 화면에 표시될 시간(초)입니다.</param>
        /// <returns>전송된 브로드캐스트 메시지의 포맷된 문자열입니다.</returns>
        public static string BroadcastToAll(string message, string color, string size,ushort duration)
        {
            string formattedMessage = $"<size={size}><color={color}>{message}</color></size>";
            foreach (Player player in Player.List)
            {
                player.Broadcast(duration, formattedMessage);
            }
            
            return $"메시지가 모든 유저한테 전송되었습니다: {formattedMessage}";
        }
    }
}