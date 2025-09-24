using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods.Music;
using RemoteAdmin;
using UnityEngine;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System.Threading.Tasks;
using GhostPlugin.Commands.MusicCommand;

namespace GhostPlugin.Commands.Jukebox
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ManageJukebox : ICommand
    {
        public string Command { get; } = "Manage_speaker";

        public string[] Aliases { get; } = { "MaSp" };

        public string Description { get; } = "Install and Play or Stop and Remove Jukebox!\n(Supports YouTube URL)";

        public static JukeboxManagement JukeboxManagement = new JukeboxManagement();
        // Insert Plugin Dir / Workspace Dir
        private readonly AudioCommands _audio = new AudioCommands(
            Plugin.Instance.AudioDirectory,
            "/home/Omega/steamcmd/scpsl/tmp-audio"
        );


        private static readonly List<string> AllowedGroups = new List<string>()
        {
            "후원자-(donator)",
            "ADMIN",
            "SERVER OWNER",
            "부서버장"
        };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string subCommand = arguments.At(0).ToLower();
            if (arguments.Count < 1)
            {
                response = "use: Manage_speaker spawn <File Name> or Manage_speaker remove <ID>";
                return false;
            }

            if (subCommand == "spawn")
            {
                return SpawnSpeaker(arguments, sender, out response);
            }
            
            else if (subCommand == "remove")
            {
                return DeleteSpeaker(arguments, out response);
            }
            response = "Unknown subcommand. useage: Manage_speaker spawn <Filename / URL> or Manage_speakert remove <ID>";
            return false;
        }

        private bool SpawnSpeaker(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender playerSender)
            {
                response = "This command is available only to players.";
                return false;
            }

            Player player = Player.Get(playerSender);
            if (player == null)
            {
                response = "Cannot Find a Player.";
                return false;
            }
            
            /*if (player.Group == null || !AllowedGroups.Contains(player.Group.BadgeText))
            {
                response = "이 명령어를 사용할 권한이 없습니다.";
                return false;
            }*/

            if (arguments.Count < 2)
            {
                response = "How to use: manage_speaker spawn <곡 이름>";
                return false;
            }
            
            string schematicName = "Speaker";
            Vector3 spawnPosition = player.Position + player.Transform.forward * 1 + player.Transform.up;
            Vector3 rotation = Vector3.forward;
            
            var songTokens = arguments.Skip(1);
            string inputSong = string.Join(" ", songTokens);

            // URL인지 판별
            bool isUrl = Uri.TryCreate(inputSong, UriKind.Absolute, out var uri)
                         && (uri.Host.IndexOf("youtube.com", StringComparison.OrdinalIgnoreCase) >= 0
                             || uri.Host.IndexOf("youtu.be", StringComparison.OrdinalIgnoreCase) >=0 );
            //string inputSong = string.Join(" ", songTokens);
            string audioDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");
            string filePath = Path.Combine(audioDirectory, inputSong);
            
            SchematicObject schematicObject = ObjectSpawner.SpawnSchematic(schematicName, spawnPosition, rotation);
            
            int id = Plugin.Instance.CurrentId++;
            Plugin.Instance.Speakers[id] = schematicObject; 
            Log.Info($"Schematic '{schematicName}' has been successfully spawned.");
            GameObject schematicGameObject = schematicObject.gameObject;
            Rigidbody rigidbody = schematicGameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = true;
            if (isUrl)
            {
                // URL: 비동기 다운로드 후 해당 스피커에서 재생
                response = $"Speaker(ID: {id}) Spawned. Starting downloading youtube video...";
                _ = DownloadAndPlayAsync(uri.ToString(), schematicObject.transform.position, id);
                return true;
            }
            else
            {
                JukeboxManagement.PlayMusicSpeaker(filePath, schematicObject.transform.position, id);
                response = $"Speaker(ID: {id}) Spawned. Location: {schematicObject.transform.position}";
                return true;
            }
        }
        
        private async Task DownloadAndPlayAsync(string youtubeUrl, Vector3 pos, int speakerId)
        {
            try
            {
                var fileName = await _audio.PrepareFileFromYouTubeAsync(youtubeUrl); // "xxx.ogg"
                if (string.IsNullOrEmpty(fileName))
                {
                    Log.Error($"[Jukebox] 유튜브 다운로드/변환 실패: {youtubeUrl}");
                    return;
                }

                string filePath = Path.Combine(Plugin.Instance.AudioDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    Log.Error($"[Jukebox] 변환된 파일을 찾을 수 없음: {filePath}");
                    return;
                }

                // 해당 스피커 ID에서 재생
                JukeboxManagement.PlayMusicSpeaker(filePath, pos, speakerId);
                Log.Info($"[Jukebox] 스피커 {speakerId} 재생 시작: {fileName}");
            }
            catch (Exception ex)
            {
                Log.Error($"[Jukebox] 유튜브 처리 중 예외: {ex}");
            }
        }


        private bool DeleteSpeaker(ArraySegment<string> arguments, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "Use: manage_speaker remove <ID>";
                return false;
            }
            
            if (!int.TryParse(arguments.At(1), out int id))
            {
                response = "ID must be a number.";
                return false;
            }
            
            if (Plugin.Instance.Speakers.TryGetValue(id, out SchematicObject schematicObject))
            {
                if (schematicObject != null && schematicObject.gameObject != null)
                {
                    UnityEngine.Object.Destroy(schematicObject.gameObject);
                }
                Plugin.Instance.Speakers.Remove(id);
                JukeboxManagement.StopMusicSpeaker(id);
                response = $"Speaker(ID: {id}) is remove succesful.";
                return true;
            }

            response = $"ID {id} Speaker is not exists.";
            return false;
        }
    }
}