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
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MERSCH : ICommand
    {
        public string Command => "MErSpawnPrim";
        public string[] Aliases => new string[] { "Mspm" };
        public string Description => "MER 네트워크에서 동기화되는 프리미티브를 소환합니다.";

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

            // 생성 위치 (플레이어 앞 2m)
            Vector3 spawnPosition = player.Position + player.Transform.forward * 2f;

            // CreateObject() 역할 수행
            PrimitiveObjectToy primitive = CreatePrimitive(primitiveType, spawnPosition, player.GameObject.transform);

            if (primitive != null)
            {
                response = $"{primitiveType} 오브젝트가 {spawnPosition}에 생성되었습니다!";
                return true;
            }

            response = "Primitive 생성에 실패했습니다.";
            return false;
        }

        // MER의 CreateObject()처럼 프리미티브 생성
        private PrimitiveObjectToy CreatePrimitive(PrimitiveType type, Vector3 position, Transform parent)
        {
            // GameObject.CreatePrimitive()로 프리미티브 생성
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.localScale = Vector3.one;

            // PrimitiveObjectToy 컴포넌트 추가 (프리미티브 상태 관리)
            PrimitiveObjectToy primitive = obj.AddComponent<PrimitiveObjectToy>();

            // 상태 설정 (PrimitiveFlags.Visible + PrimitiveFlags.Collidable)
            primitive.NetworkPrimitiveType = type;
            primitive.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

            // 색상 설정
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                renderer.material.color = randomColor;
                primitive.NetworkMaterialColor = randomColor;

                Log.Info($"[DEBUG] 색상 설정 성공: {randomColor}");
            }
            else
            {
                Log.Warn("[WARN] Renderer를 찾을 수 없습니다.");
            }

            // Rigidbody 추가 (필요한 경우)
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = true;

            // NetworkIdentity 추가 (Mirror에서 네트워크 동기화 필요)
            obj.AddComponent<NetworkIdentity>();

            // 네트워크에 동기화
            NetworkServer.Spawn(obj);

            // 디버그 출력
            Log.Info($"[DEBUG] {type} 프리미티브가 {position} 위치에 생성됨");
            Log.Info($"[DEBUG] Flags: {primitive.NetworkPrimitiveFlags}");

            // 자동 삭제 (10초 후)
            Object.Destroy(obj, 10f);

            return primitive;
        }
    }
}
