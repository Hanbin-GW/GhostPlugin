using System;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class BulletExplosion : MonoBehaviour
    {
        private Player _player;
        private Rigidbody rb;
        private bool hasCollided = false;

        public void Initialize(Player attacker)
        {
            _player = attacker;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void OnCollisionEnter_2(Collision collision)
        {
            try
            {
                if (!hasCollided) hasCollided = true;
                else return;

                if (_player == null)
                {
                    Log.Error("BulletExplosion: _player is null!");
                    return;
                }

                if (collision == null || collision.collider == null || collision.collider.gameObject == null)
                {
                    Log.Error("BulletExplosion: Collision or collider is null!");
                    return;
                }

                // 자기 자신 무시
                if (collision.collider.gameObject == _player.GameObject)
                {
                    Log.Debug("BulletExplosion: 자기 자신과 충돌 - 무시됨");
                    return;
                }

                // 이펙트/수류탄 같은 물체랑 충돌 시 무시
                if (collision.collider.gameObject.TryGetComponent<EffectGrenade>(out _))
                {
                    Log.Debug("BulletExplosion: 다른 이펙트/수류탄과 충돌 - 무시됨");
                    return;
                }

                ContactPoint contact = collision.contacts[0];
                Vector3 spawnPoint = contact.point + contact.normal * 0.05f;

                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                transform.position = spawnPoint;
                transform.forward = -contact.normal;

                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 0.8f;
                grenade.ChangeItemOwner(Server.Host, _player);
                grenade.SpawnActive(spawnPoint);

                Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());
                if (target != null && target != _player)
                {
                    _player.ShowHitMarker();
                }

                UnityEngine.Object.Destroy(gameObject, 2f);
            }
            catch (Exception ex)
            {
                Log.Error($"[BulletExplosion] OnCollisionEnter error:\n{ex}");
                UnityEngine.Object.Destroy(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (hasCollided) return;
            hasCollided = true;

            ContactPoint contact = collision.contacts[0];
            Vector3 spawnPoint = contact.point + contact.normal * 0.05f;

            // 자기 자신 충돌 무시
            Player target = Player.Get(collision.collider) ??
                            Player.Get(collision.collider.GetComponentInParent<Collider>());
            if (target != null && target == _player)
            {
                Log.Info("자기 자신과 충돌한 것으로 감지됨. 무시됨.");
                return;
            }

            // 정지 처리
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 1. 폭발 생성
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 0.8f;
            grenade.ChangeItemOwner(Server.Host, _player);
            grenade.SpawnActive(spawnPoint); // 💥 수정된 정확한 지점

            // 2. 유탄도 붙이기 (시각용)
            transform.position = spawnPoint;
            transform.forward = -contact.normal; // 시각적으로 벽을 향하게

            if (target != null && target != _player)
                _player.ShowHitMarker();

            Destroy(gameObject, 2f);
        }
    }

}