using System;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods.Music;

namespace GhostPlugin.Commands.MusicCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class YtPlayCommand : ICommand
    {
        public string Command { get; } = "ytplay";
        public string[] Aliases { get; } = { "ytmusic" };
        public string Description { get; } = "유튜브 URL을 다운로드하여 음악을 재생합니다.";

        private readonly MusicManager _musicManager = 
            new MusicManager(Plugin.Instance.AudioDirectory, "/home/vscode/steamcmd/scpsl/tmp-audio");

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "사용법: ytplay <유튜브URL>";
                return false;
            }

            string url = arguments.At(0);

            // 비동기 실행 (Execute는 async 불가)
            _ = Task.Run(async () =>
            {
                try
                {
                    var fileName = await _musicManager.audioCommands.PrepareFileFromYouTubeAsync(url);
                    if (fileName == null)
                    {
                        Log.Error("유튜브 다운로드/변환 실패");
                        return;
                    }

                    _musicManager.PlaySpecificMusic(fileName);
                }
                catch (Exception ex)
                {
                    Log.Error($"[ytplay] 오류: {ex}");
                }
            });

            response = $"유튜브 다운로드 시작: {url}";
            return true;
        }
    }
}