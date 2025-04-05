using System;
using System.Collections;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class BulletExplosion
    {
        private Player _player;
        
        public void Initialize(int damage, Player attacker)
        {
            _player = attacker;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPosition = contact.point;

            // 이펙트 출력
            SpawnExplosionEffect(hitPosition);

            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());
            if (target != null && target != _player)
            {
                _player.ShowHitMarker(2);
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 1f;
                grenade.ChangeItemOwner(Server.Host, _player);
                grenade.SpawnActive(hitPosition);
            }
        }
        IEnumerator CreateTrailEffect(Vector3 startPos, Vector3 direction, int count, float spacing)
        {
            PrimitiveObjectToy toy = null;
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = startPos + direction.normalized * spacing * i;
                // ← 각 복사본은 처음 위치에서 direction 방향으로 조금씩 더 멀어짐

                foreach (GameObject value in Mirror.NetworkClient.prefabs.Values)
                {
                    if (value.TryGetComponent(out PrimitiveObjectToy component))
                    {
                        toy = Object.Instantiate(component);
                        toy.OnSpawned(Server.Host.ReferenceHub, new ArraySegment<string>(Array.Empty<string>()));
                        break;
                    }
                }

                if (toy != null) 
                    toy.transform.position = spawnPos;

                yield return new WaitForSeconds(0.05f);
            }
        }

        private void SpawnExplosionEffect(Vector3 position)
        {
            GameObject effectPrefab = Resources.Load<GameObject>("Effects/ExplosionEffect"); // Resources/Effects/ExplosionEffect.prefab
            if (effectPrefab != null)
            {
                GameObject effectInstance = Object.Instantiate(effectPrefab, position, Quaternion.identity);
                Object.Destroy(effectInstance, 3f);
            }
        }

    }
}