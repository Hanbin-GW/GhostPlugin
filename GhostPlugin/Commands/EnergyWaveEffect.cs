using System;
using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Mirror;
using UnityEngine;

namespace GhostPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EnergyWaveEffect : ICommand
    {
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");

        public string Command => "EnergyWaveEffect";
        public string[] Aliases => new[] { "EWE" };
        public string Description => "Spawn a Energy Effect";

        private const int SpawnCount = 10;
        private const float SpawnRange = 2.0f;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (player == null)
            {
                response = "플레이어를 찾을 수 없습니다.";
                return false;
            }

            for (int i = 0; i < SpawnCount; i++)
            {
                PrimitiveObjectToy pObject = null;

                foreach (GameObject value in NetworkClient.prefabs.Values)
                {
                    if (value.TryGetComponent<PrimitiveObjectToy>(out var component))
                    {
                        pObject = UnityEngine.Object.Instantiate(component);
                        pObject.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                        break;
                    }
                }

                if (pObject != null)
                {
                    pObject.NetworkPrimitiveType = PrimitiveType.Cube;
                    pObject.transform.localScale = Vector3.one * 0.05f;
                    pObject.NetworkScale = Vector3.one * 0.05f;
                    pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

                    Vector3 randomOffset = new Vector3(
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange),
                        UnityEngine.Random.Range(1f, 3f), // 높이 랜덤값
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange)
                    );

                    pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;

                    Color glowColor = new Color(1f, 0.0f, 0.0f, 0.1f) * 50f;
                    pObject.NetworkMaterialColor = glowColor;
                    pObject.MaterialColor = glowColor;

                    var rb = pObject.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = pObject.gameObject.AddComponent<Rigidbody>();

                    rb.useGravity = true;
                    rb.mass = 1f;
                    rb.drag = 0.5f;
                    rb.angularDrag = 0.1f;

                    var collider = pObject.GetComponent<Collider>();
                    if (collider == null)
                        collider = pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 15f);
                }
            }

            response = $"{SpawnCount} Glow Primitives spawned\nThey will be removed after 15 seconds...";
            return true;
        }
    }
}
