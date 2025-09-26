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

        public string Description { get; } = "소환 위치에 스피커 주크박스를 관리합니다.";

        public static JukeboxManagement JukeboxManagement = new JukeboxManagement();
        // 플러그인 경로/워크디렉토리 주입
        /*private readonly AudioCommands _audio = new AudioCommands(
            Plugin.Instance.AudioDirectory,
            "/home/vscode/steamcmd/scpsl/tmp-audio"
        );*/
        static string audioDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EXILED", "Plugins", "audio");
        static string workDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EXILED", "Plugins", "tmp-audio");

        private readonly AudioCommands _audio = new AudioCommands(audioDir, workDir);

        private static readonly List<string> AllowedGroups = new List<string>()
        {
            "후원자-(donator)",
            "서버 관리자",
            "서버 운영자",
            "부서버장"
        };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string subCommand = arguments.At(0).ToLower();
            if (arguments.Count < 1)
            {
                response = "사용법: Manage_speaker spawn <곡 이름> 또는 Manage_speaker remove <ID>";
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
            response = "알 수 없는 하위 명령어입니다. 사용법: Manage_speaker spawn 또는 Manage_speakert remove";
            return false;
        }

        private bool SpawnSpeaker(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender playerSender)
            {
                response = "이 명령어는 플레이어만 사용할 수 있습니다.";
                return false;
            }

            Player player = Player.Get(playerSender);
            if (player == null)
            {
                response = "플레이어를 찾을 수 없습니다.";
                return false;
            }
            
            /*if (player.Group == null || !AllowedGroups.Contains(player.Group.BadgeText))
            {
                response = "이 명령어를 사용할 권한이 없습니다.";
                return false;
            }*/
            

            if (arguments.Count < 2)
            {
                response = "사용법: manage_speaker spawn <곡 이름>";
                return false;
            }
            
            string schematicName = "Speaker";
            Vector3 spawnPosition = player.Position + player.Transform.forward * 1 + player.Transform.up;
            Quaternion rotation = player.Transform.rotation;

            if (AllowedGroups.Contains(player.Group.BadgeText))
            {
                schematicName = "LargeSpeaker";
            }
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
                response = $"스피커(ID: {id}) 생성됨. 유튜브 플래이백 시작…";
                _ = DownloadAndPlayAsync(uri.ToString(), schematicObject.transform.position, id);
                return true;
            }
            else
            {
                JukeboxManagement.PlayMusicSpeaker(filePath, schematicObject.transform.position, id);
                response = $"스피커(ID: {id})가 생성되었습니다. 위치: {schematicObject.transform.position}";
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
                response = "사용법: manage_speaker remove <ID>";
                return false;
            }
            
            if (!int.TryParse(arguments.At(1), out int id))
            {
                response = "ID는 숫자여야 합니다.";
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
                response = $"스피커(ID: {id})가 제거되었습니다.";
                return true;
            }

            response = $"ID {id}에 해당하는 스피커가 없습니다.";
            return false;
        }
    }
}