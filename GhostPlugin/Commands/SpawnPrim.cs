using System;
using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Mirror;
using RemoteAdmin;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))] // 관리자 전용 명령어
    public class SpawnPrim : ICommand
    {
        public string Command => "SpawmPrim";
        public string[] Aliases => new string[] { "spm" };
        public string Description => "네트워크에서 동기화되는 프리미티브를 소환합니다.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "이 명령어는 플레이어만 사용할 수 있습니다!";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            if (player == null)
            {
                response = "플레이어 정보를 가져올 수 없습니다.";
                return false;
            }

            // 기본 값 설정 (큐브)
            PrimitiveType primitiveType = PrimitiveType.Cube;

            // 명령어 인자를 통해 유형 결정
            if (arguments.Count > 0)
            {
                string typeInput = arguments.At(0).ToLower();
                switch (typeInput)
                {
                    case "cube":
                        primitiveType = PrimitiveType.Cube;
                        break;
                    case "sphere":
                        primitiveType = PrimitiveType.Sphere;
                        break;
                    case "cylinder":
                        primitiveType = PrimitiveType.Cylinder;
                        break;
                    default:
                        response = "잘못된 오브젝트 타입입니다! 사용 가능한 값: cube, sphere, cylinder";
                        return false;
                }
            }

            Vector3 spawnPosition = player.Position + player.Transform.forward * 1f;
            var primitive = Exiled.API.Features.Toys.Primitive.Create(primitiveType, spawnPosition, new Vector3(0, 0, 0),
                new Vector3(1, 1, 1), true);
            
            if (primitive != null)
            {
                // 상태 설정
                primitive.Flags = PrimitiveFlags.Collidable | PrimitiveFlags.Visible;

                // 색상 설정
                Renderer renderer = primitive.GameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(Random.value, Random.value, Random.value);
                }

                NetworkServer.Spawn(primitive.GameObject);
                // 자동 삭제
                Object.Destroy(primitive.GameObject, 10f);
            }
            response = $"{primitiveType} 오브젝트가 {spawnPosition}에 생성되었습니다!";
            return true;
        }
    }

}