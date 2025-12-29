using System;
using System.IO;
using CommandSystem;
using GhostPlugin.Methods.Music;

namespace GhostPlugin.Commands.MusicCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class LiveStreamCommand : ICommand
    {
        public string Command { get; } = "LivePlayMusic";
        public string[] Aliases { get; } = new[] { "lpm" };
        public string Description { get; } = "라이브 스트리밍 커맨드!";
        private readonly MusicMethods musicMethods = 
            new MusicMethods(Plugin.Instance.AudioDirectory, "/data/scpsl/tmp-audio");
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "사용법: lpm <url>";
                return false;
            }

            string urlName = arguments.At(0);  // 첫 번째 인자로 파일 이름을 가져옴
            
            if (urlName == String.Empty)
            {
                response = $"파일을 찾을 수 없습니다: {urlName}";
                return false;
            }

            musicMethods.PlayPreparedAlias(urlName);
            response = $"음악을 재생합니다: {urlName}";
            return true;
        }
    }
}