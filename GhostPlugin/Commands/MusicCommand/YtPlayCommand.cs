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
        public string Description { get; } = "Play a YouTube URL to play music.";

        private readonly MusicMethods _musicMethods = 
            new MusicMethods(Plugin.Instance.AudioDirectory, "/home/Omega/steamcmd/scpsl/tmp-audio");

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Use: ytplay <Youtube URL>";
                return false;
            }

            string url = arguments.At(0);

            // 비동기 실행 (Execute는 async 불가)
            _ = Task.Run(async () =>
            {
                try
                {
                    var fileName = await _musicMethods.audioCommands.PrepareFileFromYouTubeAsync(url);
                    if (fileName == null)
                    {
                        Log.Error("Failed to Playback/convert YouTube");
                        return;
                    }

                    MusicMethods.PlaySpecificMusic(fileName);
                }
                catch (Exception ex)
                {
                    Log.Error($"[ytplay] Error: {ex}");
                }
            });

            response = $"Playback Start: {url}\nplease Give a time to play";
            return true;
        }
    }
}