using System;
using System.Collections.Generic;
using System.IO;
using CommandSystem;
using Exiled.API.Features;
using GhostPlugin.Methods;
using RemoteAdmin;
using UnityEngine;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;

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

        private static readonly List<string> AllowedGroups = new List<string>()
        {
            "donator",
            "admin",
            "owner",
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
            
            if (player.Group == null || !AllowedGroups.Contains(player.Group.BadgeText))
            {
                response = "이 명령어를 사용할 권한이 없습니다.";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "사용법: manage_speaker spawn <곡 이름>";
                return false;
            }

            string schematicName = "LargeSpeaker";
            Vector3 spawnPosition = player.Position + player.Transform.forward * 1 + player.Transform.up;
            Quaternion rotation = Quaternion.identity;
            
            string inputSong = string.Join(" ", arguments);
            string audioDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");
            string filePath = Path.Combine(audioDirectory, inputSong);
            
            SchematicObject schematicObject = ObjectSpawner.SpawnSchematic(schematicName, spawnPosition, rotation,null,null!);
            
            if (schematicObject != null)
            {
                int id = Plugin.Instance.CurrentId++;
                Plugin.Instance.Speakers[id] = schematicObject;
                Log.Info($"Schematic '{schematicName}' has been successfully spawned.");
                GameObject schematicGameObject = schematicObject.gameObject;
                Rigidbody rigidbody = schematicGameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
                JukeboxManagement.PlayMusicSpeaker(filePath,schematicObject.Position,id);
                response = $"스피커(ID: {id})가 생성되었습니다. 위치: {schematicObject.Position}";
                return true;
            }

            response = "스피커를 생성하지 못했습니다.";
            return false;
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
                schematicObject.Destroy();
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