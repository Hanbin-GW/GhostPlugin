using System;
using System.Collections;
using AdminToys;
using Exiled.API.Features;
using GhostPlugin.Custom.Items.MonoBehavior;
using Mirror;
using UnityEngine;
namespace GhostPlugin.Methods.Objects
{
    public class SpawnParticleSpark
    {
        private const int SpawnCount = 5;
        private const float SpawnRange = 2.0f;
        
        
        /// <summary>
        /// Enersize Ammo
        /// </summary>
        /// <param name="player">Attacker</param>
        /// <param name="position">object crate position</param>
        /// <param name="forwardForce">speed</param>
        /// <param name="spawnRange"></param>
        /// <param name="glowColor">Color</param>
        /// <returns></returns>
        public PrimitiveObjectToy SpawnGrenade(Player player, Vector3 position, float forwardForce, float spawnRange, Color glowColor)
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

                /*Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-spawnRange, spawnRange),
                    0.5f,
                    UnityEngine.Random.Range(-spawnRange, spawnRange)
                );*/
                Vector3 spawnPos = position + player.GameObject.transform.forward * 3f;

                pObject.transform.position = spawnPos;
                pObject.Position = spawnPos;
                pObject.NetworkPosition = spawnPos;

                pObject.NetworkMaterialColor = glowColor;
                pObject.MaterialColor = glowColor;

                var rb = pObject.GetComponent<Rigidbody>() ?? pObject.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 1f;
                rb.drag = 0.5f;
                rb.angularDrag = 0.1f;
                Vector3 shootDirection = player.GameObject.transform.forward;
                rb.velocity = shootDirection * forwardForce;

                var collider = pObject.GetComponent<Collider>() ?? pObject.gameObject.AddComponent<BoxCollider>();

                Collider[] playerColliders = player.GameObject.GetComponentsInChildren<Collider>();
                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(collider, playerCol, true);
                }
                Log.Debug("발사자의 모든 Collider와 총알 간 충돌 무시 처리 완료.");
                Log.Debug($"[Spawn] Bullet position: {position}");
                Log.Debug($"[Actual set] transform: {pObject.transform.position}");
                var bulletCollision = pObject.gameObject.AddComponent<BulletExplosion>();
                bulletCollision.Initialize(player);

                UnityEngine.Object.Destroy(pObject.gameObject, 5f);
            }

            return pObject;
        }

        
        
        /// <summary>
        /// Used in plasma shotgun
        /// </summary>
        /// <param name="player"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public PrimitiveObjectToy SpawnEnergyGuage(Player player, Vector3 position)
        {
            PrimitiveObjectToy pObject = null;
            float forwardForce = 20f;
            foreach (GameObject value in NetworkClient.prefabs.Values) 
            {
                if (value.TryGetComponent<PrimitiveObjectToy>(out var component)) {
                    pObject = UnityEngine.Object.Instantiate(component); 
                    pObject.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0])); 
                    break;
                }
            }
            
            if (pObject != null) 
            { 
                pObject.NetworkPrimitiveType = PrimitiveType.Cube; 
                pObject.transform.localScale = Vector3.one * 0.15f; 
                pObject.NetworkScale = Vector3.one * 0.15f; 
                pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

                //pObject.Position = position + player.CameraTransform.forward + randomOffset;
                //pObject.Position = position + player.GameObject.transform.forward * 1.1f;
                //pObject.NetworkPosition = position + player.GameObject.transform.forward * 1.1f;
                Vector3 spawnOffset = player.GameObject.transform.forward * 2.0f;
                pObject.Position = position + spawnOffset;
                pObject.NetworkPosition = position + spawnOffset;
                //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                Color glowColor = new Color(0.0f, 0.0f, 1.0f, 0.1f) * 50f;
                pObject.NetworkMaterialColor = glowColor; 
                pObject.MaterialColor = glowColor;
                    
                var rb = pObject.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = pObject.gameObject.AddComponent<Rigidbody>();

                rb.useGravity = true;
                rb.isKinematic = false;
                rb.mass = 1f; 
                rb.drag = 0.5f; 
                rb.angularDrag = 0.1f;

                Vector3 shootDirection = player.GameObject.transform.forward;
                rb.velocity = shootDirection * forwardForce; 
                //Test Code
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                /*var collider = pObject.GetComponent<Collider>(); 
                if (collider == null)  
                    pObject.gameObject.AddComponent<BoxCollider>();*/
                
                CharacterController playerController = player.GameObject.GetComponent<CharacterController>();
                Collider bulletCollider = pObject.GetComponent<Collider>();

                if (playerController != null && bulletCollider != null)
                {
                    Physics.IgnoreCollision(bulletCollider, playerController, true);
                }
                
                UnityEngine.Object.Destroy(pObject.gameObject, 10f);
            } 
            return pObject;
        }

        /// <summary>
        /// used empty shell
        /// </summary>
        /// <param name="player"></param>
        /// <param name="position"></param>
        /// <param name="glowColor"></param>
        public void SpawnSpark(Player player, Vector3 position,Color glowColor)
        {
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
                    pObject.transform.localScale = Vector3.one * 0.03f;
                    pObject.NetworkScale = Vector3.one * 0.03f;
                    pObject.NetworkPrimitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable;

                    Vector3 randomOffset = new Vector3(
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange),
                        UnityEngine.Random.Range(1f, 3f), // 높이 랜덤값
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange)
                    );

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = position + player.GameObject.transform.forward * 1.05f + randomOffset;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    
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
                        pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 10f);
                }
            }
        }
        
        public void SpawnREDSpark(Player player, Vector3 position,float spawncount)
        {
            for (int i = 0; i < spawncount; i++)
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
                        UnityEngine.Random.Range(2f, 3f), // 높이 랜덤값
                        UnityEngine.Random.Range(-SpawnRange, SpawnRange)
                    );

                    //pObject.Position = player.Position + player.GameObject.transform.forward + randomOffset;
                    pObject.Position = position + player.GameObject.transform.forward + randomOffset;
                    //Color glowColor = new Color(0.0f, 1.0f, 1.0f, 0.1f) * 50f;
                    Color glowColor = new Color(1.0f, 0.0f, 0.0f,0.1f) * 50f;
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
                        pObject.gameObject.AddComponent<BoxCollider>();

                    UnityEngine.Object.Destroy(pObject.gameObject, 10f);
                }
            }
        }
        /// <summary>
        /// Not Used
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        private IEnumerator ShrinkAndDestroyCube(PrimitiveObjectToy cube)
        {
            while (cube.transform.localScale.x > 0.05f)
            {
                cube.transform.localScale -= Vector3.one * 0.1f * Time.deltaTime;
                yield return null;
            }
            UnityEngine.Object.Destroy(cube.gameObject);
        }
    }
}