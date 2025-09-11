using System;
using System.Collections.Generic;
using System.IO;
using CommandSystem;
using Exiled.API.Features;

namespace GhostPlugin.Commands.Jukebox
{
    public class MusicList
    {
        [CommandHandler(typeof(ClientCommandHandler))]
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class List : ICommand
        {
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                response = $"Successfully music list has been printed!\n{ListMusicFiles()}";
                return true;
            }

            public string Command { get; } = "MusicList";
            public string[] Aliases { get; } = new[] {"MusicList","ML","mli"};
            public string Description { get; } = "Print a list of Music";
            
            public string ListMusicFiles()
            {
                List<string> stringBuilder = new List<string>();
                //StringBuilder fileListBuilder = new StringBuilder();
                if (Directory.Exists(Plugin.Instance.AudioDirectory))
                {
                    string[] musicFiles = Directory.GetFiles(Plugin.Instance.AudioDirectory, "*.ogg");  // .ogg 파일만 가져옴
                    if (musicFiles.Length > 0)
                    {

                        //Log.Info($"음악 파일 목록 (총 {musicFiles.Length}개):");
                        foreach (string file in musicFiles)
                        {
                            string fileName = Path.GetFileName(file);
                            stringBuilder.Add(fileName);
                        }
                    }
                    else
                    {
                        Log.Warn("Cannot find file in folder.");
                    }
                }
                else
                {
                    Log.Error($"Cannot find music folder: {Plugin.Instance.AudioDirectory}.");
                    //Directory.CreateDirectory(audioDirectory);
                }
                string finalFileList = string.Join("\n", stringBuilder);
                return finalFileList;
            }
        }
    }
}