using AdminToys;
using GhostPlugin.Custom.Items.MonoBehavior;
using UnityEngine;
using Player = Exiled.API.Features.Player;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;

namespace GhostPlugin.Methods.Objects
{
    public class Cube
    {
        public static void SpawnAcidAmmo(Player player, Vector3 position, int count,
            float forwardForce, float spawnRange, Color glowColor)
        {
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-spawnRange, spawnRange),
                UnityEngine.Random.Range(-spawnRange, spawnRange), // 높이 랜덤값
                UnityEngine.Random.Range(-spawnRange, spawnRange)
            );
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = position + Random.insideUnitSphere * spawnRange;

                PrimitiveObjectToy toy = PrimitiveObjectToy.Create(spawnPos, Quaternion.identity, Vector3.one);
                toy.Scale = Vector3.one * 0.05f;
                toy.Flags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;
                toy.Type = PrimitiveType.Cube;
                toy.Color = glowColor;
                toy.Base.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;
                toy.Base.NetworkPosition = position + player.GameObject.transform.forward * 1.5f + randomOffset;

                // forwardForce 적용
                Rigidbody rb = toy.Base.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(player.ReferenceHub.transform.forward * forwardForce, ForceMode.Impulse);
                }
                var collision = toy.Base.gameObject.AddComponent<PoisonBulletCollision>();
                collision.Initialize(player);
                Object.Destroy(toy.GameObject, 2f);
            }
        }
    }
}