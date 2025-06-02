using System;
using AdminToys;
using Exiled.API.Features;
using Mirror;
using UnityEngine;

namespace GhostPlugin.Methods.Objects
{
    public class SpawnPrimitiveToy
    {
        private const float SpawnRange = 2.0f;

        public static void Spawn(Player player,int count)
        {
            for (int i = 0; i < count; i++)
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
                        UnityEngine.Random.Range(1f, 3f),
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange)
                    );

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = player.Position;

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
        }
    }
}