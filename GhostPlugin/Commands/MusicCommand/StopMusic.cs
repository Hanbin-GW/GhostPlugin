using System;
using CommandSystem;
using GhostPlugin.Methods.Music;

namespace GhostPlugin.Commands.MusicCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StopMusic : ICommand
    {
        public string Command => "StopMusic";
        public string[] Aliases { get; } =  new []{"sm"};
        public string Description => "현재 재생 중인 음악을 중지합니다.";
        private readonly MusicManager _musicManager = 
            new MusicManager(Plugin.Instance.AudioDirectory, "/home/vscode/steamcmd/scpsl/tmp-audio");
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance == null)
            {
                response = "현재 재생 중인 음악이 없습니다.";
                return false;
            }

            // 음악을 중지하는 로직 호출
            MusicManager.StopMusic();
            response = "음악이 중지되었습니다.";
            return true;
        }
    }
}