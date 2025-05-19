using System;
using AdminToys;
using Exiled.API.Features;
using GhostPlugin.Custom.Items.MonoBehavior;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Methods.Objects
{
    public class SpawmPlasma
    {
        private const float BlockSize = 0.05f;
        private const float Speed = 30f;

        public void SpawnPlasma(Player player)
        {
            Vector3 startPosition = player.CameraTransform.position;
            Vector3 direction = player.CameraTransform.forward.normalized;

            SpawnLaserObjectInstance(player, startPosition, direction);
        }

        private void SpawnLaserObjectInstance(Player player, Vector3 startPosition, Vector3 direction)
        {
            PrimitiveObjectToy pObject = null;

            foreach (GameObject value in Mirror.NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent(out PrimitiveObjectToy component))
                {
                    pObject = Object.Instantiate(component);
                    pObject.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                    break;
                }
            }

            if (pObject != null)
            {
                pObject.NetworkPosition = startPosition;
                pObject.Position = startPosition;
                pObject.transform.position = startPosition;

                pObject.NetworkPrimitiveType = PrimitiveType.Cube;
                pObject.NetworkScale = Vector3.one * BlockSize;
                pObject.Scale = Vector3.one * BlockSize;
                pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

                Color glowColor = new Color(1.0f, 0.5f, 0.0f, 0.1f) * 50f;
                pObject.NetworkMaterialColor = glowColor;
                var bulletcollision = pObject.gameObject.AddComponent<FireBulletCollision>();
                bulletcollision.Initialize(90, player);
                // ✅ Rigidbody로 이동 설정
                Rigidbody rb = pObject.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = pObject.gameObject.AddComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                rb.useGravity = false;   // 중력 비활성화
                rb.isKinematic = false;  // 물리 이동 활성화
                rb.velocity = direction * Speed; // ✅ 초당 200m 속도 설정
                
                // ✅ Collider 삭제 (충돌 방지)
                Collider collider = pObject.GetComponent<Collider>();
                if (collider != null)
                    Object.Destroy(collider);

                // ✅ 정확히 1초 후 삭제
                Object.Destroy(pObject.gameObject, 1f);
            }
        }
    }
}