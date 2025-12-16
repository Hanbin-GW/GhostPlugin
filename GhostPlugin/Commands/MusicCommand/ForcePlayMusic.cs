using System;
using System.IO;
using CommandSystem;
using GhostPlugin.Methods.Music;

namespace GhostPlugin.Commands.MusicCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ForcePlayMusic
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "사용법: Pm <파일이름.ogg>";
                return false;
            }
            string fileName = arguments.At(0);  // 첫 번째 인자로 파일 이름을 가져옴
            string audioDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");
            string filePath = Path.Combine(audioDirectory, fileName);
            
            if (!File.Exists(filePath))
            {
                response = $"파일을 찾을 수 없습니다: {filePath}";
                return false;
            }

            MusicMethods.PlaySpecificMusic(filePath);
            response = $"음악을 재생합니다: {fileName}";
            return true;
        }

        private readonly MusicMethods _musicMethods = 
            new MusicMethods(Plugin.Instance.AudioDirectory, "/home/vscode/steamcmd/scpsl/tmp-audio");
        public string Command { get; } = "ForcePlayMusic";
        public string[] Aliases { get; } = new[] { "Fpm", "ForcePlayMusic", "FPlaym" };
        public string Description { get; } = "특정곡을 강제로 재생합니다!";
    }
}