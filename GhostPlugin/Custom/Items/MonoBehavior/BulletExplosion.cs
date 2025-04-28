using System;
using System.Collections;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class BulletExplosion : MonoBehaviour
    {
        private Player _player;
        
        public void Initialize(Player attacker)
        {
            _player = attacker;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPosition = contact.point;

            Log.Info($"충돌한 대상: {collision.gameObject.name}");
            
            // 타겟 충돌 처리
            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());
            if (target != null && target != _player)
            {
                _player.ShowHitMarker(2);
            }

            // 수동으로 타이머 처리 (폭발 딜레이 적용)
            StartCoroutine(SpawnDelayedGrenade(hitPosition, 1f));
        }

        
        private IEnumerator SpawnDelayedGrenade(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);

            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.ChangeItemOwner(Server.Host, _player);
            grenade.SpawnActive(position);
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
    }
}