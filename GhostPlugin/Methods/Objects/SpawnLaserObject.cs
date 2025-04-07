using System;
using AdminToys;
using Exiled.API.Features;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Methods.Objects
{
    public class SpawnLaserObject
    {
        private const float BlockSize = 0.08f;
        private const float Speed = 0f;

        public void SpawnLaser(Player player)
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
                float segmentSpacing = 0.2f; // 큐브 간 0.2 단위 거리로 직선 배치
                int segmentCount = 25;
                //Vector3 pos = startPosition + direction * segmentSpacing * 1;
                /*pObject.NetworkPosition = startPosition;
                pObject.Position = startPosition;
                pObject.transform.position = startPosition;

                pObject.NetworkPrimitiveType = PrimitiveType.Cube;
                pObject.NetworkScale = new Vector3(0.08f, 0.08f, 3f);
                pObject.Scale = Vector3.one * BlockSize;*/
                pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible;
                for (int i = 0; i < segmentCount; i++)
                {
                    Vector3 pos = startPosition + direction * segmentSpacing * 1;
                    PrimitiveObjectToy segment = Object.Instantiate(pObject);
                    segment.transform.position = pos;
                    segment.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                    segment.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                    segment.NetworkScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Color glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                    segment.MaterialColor = glowColor;
                    segment.NetworkMaterialColor = glowColor;

                    Rigidbody rb = segment.GetComponent<Rigidbody>() ?? segment.gameObject.AddComponent<Rigidbody>();
                    if (rb == null)
                        rb = segment.gameObject.AddComponent<Rigidbody>();

                    rb.useGravity = false;
                    rb.isKinematic = true;
                    Object.Destroy(segment.gameObject, 2f);
                }
            }
        }
    }
}
