using System;
using AdminToys;
using Exiled.API.Features;
using Mirror;
using UnityEngine;
using GhostPlugin.Custom.Items.MonoBehavior;
namespace GhostPlugin.Methods.Objects
{
    public class PlasmaCube
    {
        public static PrimitiveObjectToy SpawmSparkAmmo(Player player, Vector3 position, int count, float forwardForce, float spawnRange,Color glowColor)
        {
            PrimitiveObjectToy pObject = null;
            for (int i = 0; i < count; i++)
            {

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
                        UnityEngine.Random.Range(-spawnRange, spawnRange),
                        UnityEngine.Random.Range(0.5f, 0.5f), // 높이 랜덤값
                        UnityEngine.Random.Range(-spawnRange, spawnRange)
                    );

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = position + player.GameObject.transform.forward * 1.5f + randomOffset;
                    pObject.NetworkPosition = position + player.GameObject.transform.forward * 1.5f + randomOffset;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    pObject.NetworkMaterialColor = glowColor;
                    pObject.MaterialColor = glowColor;

                    var rb = pObject.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = pObject.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.mass = 1f;
                    rb.drag = 0.5f;
                    rb.angularDrag = 0.1f;
                    Vector3 shootDirection = player.GameObject.transform.forward;
                    rb.velocity = shootDirection * forwardForce; 
                    var collider = pObject.GetComponent<Collider>();
                    if (collider == null)
                        pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 1.5f);
                }
            }
            return pObject;
        }
        
        public PrimitiveObjectToy SpawmSparkAmmoNoSpread(Player player, Vector3 position, float forwardForce,Color glowColor, float scale)
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
                    pObject.transform.localScale = Vector3.one * 0.2f;
                    pObject.NetworkScale = Vector3.one * 0.2f;
                    pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = position + player.GameObject.transform.forward * 1.25f;
                    pObject.NetworkPosition = position + player.GameObject.transform.forward * 1.25f;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    pObject.NetworkMaterialColor = glowColor;
                    pObject.MaterialColor = glowColor;

                    var rb = pObject.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = pObject.gameObject.AddComponent<Rigidbody>();
                    //rb.isKinematic = false;
                    //rb.useGravity = true;
                    rb.mass = 1f;
                    rb.drag = 0.5f;
                    rb.angularDrag = 0.1f;
                    Vector3 shootDirection = player.GameObject.transform.forward;
                    rb.velocity = shootDirection * forwardForce; 
                    var collider = pObject.GetComponent<Collider>();
                    if (collider == null)
                        pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 1.5f);
                }
            return pObject;
        }
        
        public static PrimitiveObjectToy SpawmSparkBuckshot(Player player, Vector3 position, int count, float forwardForce, float spawnRange,Color glowColor)
        {
            PrimitiveObjectToy pObject = null;
            for (int i = 0; i < count; i++)
            {

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
                        UnityEngine.Random.Range(-spawnRange, spawnRange),
                        UnityEngine.Random.Range(0.5f, 0.5f), // 높이 랜덤값
                        UnityEngine.Random.Range(-spawnRange, spawnRange)
                    );

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = position + player.GameObject.transform.forward * 1.5f + randomOffset;
                    pObject.NetworkPosition = position + player.GameObject.transform.forward * 1.5f + randomOffset;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    pObject.NetworkMaterialColor = glowColor;
                    pObject.MaterialColor = glowColor;
                    ///---------------------------Temporary Code----------------------------
                    var bulletcollision = pObject.gameObject.AddComponent<FireBulletCollision>();
                    bulletcollision.Initialize(15, player);
                    ///----------------------------------------------------------------------
                    var rb = pObject.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = pObject.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.mass = 1f;
                    rb.drag = 0.5f;
                    rb.angularDrag = 0.1f;
                    Vector3 shootDirection = player.GameObject.transform.forward;
                    rb.velocity = shootDirection * forwardForce; 
                    var collider = pObject.GetComponent<Collider>();
                    if (collider == null)
                        pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 1.5f);
                }
            }
            return pObject;
        }
    }
}