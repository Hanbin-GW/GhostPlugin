using Exiled.API.Features;
using Exiled.API.Features.Items;
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

        private void OnCollisionEnter(Collision collision)
        {
            if (hasCollided) return;
            hasCollided = true;

            ContactPoint contact = collision.contacts[0];
            Vector3 explosionPosition = contact.point;

            // 유탄 고정
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 벽에 달라붙도록 살짝 전진
            transform.position += transform.forward * 0.1f;

            // 폭발 생성
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = 0.8f; // 약간의 딜레이로 폭발
            grenade.ChangeItemOwner(Server.Host, _player);
            grenade.SpawnActive(explosionPosition);

            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());
            if (target != null && target != _player)
            {
                _player.ShowHitMarker();
            }

            Destroy(gameObject, 2f);
        }
    }

}