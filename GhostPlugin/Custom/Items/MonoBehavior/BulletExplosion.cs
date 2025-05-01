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
            Vector3 explosionPosition = contact.point;

            Log.Info($"충돌 대상: {collision.gameObject.name}, 발사자: {_player.DisplayNickname}");

            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 0.5f; // 조금 더 빠른 폭발 원할 경우 0.2f 등으로 조정
            grenade.ChangeItemOwner(Server.Host, _player);
            grenade.SpawnActive(explosionPosition);

            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());
            if (target != null && target != _player)
            {
                _player.ShowHitMarker();
            }

            Destroy(gameObject, 2f);
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