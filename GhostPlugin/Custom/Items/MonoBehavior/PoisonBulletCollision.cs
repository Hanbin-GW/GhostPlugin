using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class PoisonBulletCollision : MonoBehaviour
    {
        private int damage;
        private Player _attacker;
        private bool _hasCollided = false;
        public void Initialize(int damage, Player attacker)
        {
            this.damage = damage;
            _attacker = attacker;
            Log.Debug($"[PoisonBullet] Initialized with damage: {damage}, attacker: {_attacker?.Nickname}");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasCollided) return;

            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());

            if (target != null && target != _attacker)
            {
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                _hasCollided = true;
                Log.Debug($"Hit Player: {target.Nickname}");
                
                target.EnableEffect<Decontaminating>(duration: 8,intensity:1);
                target.EnableEffect<Poisoned>(duration: 20,intensity:2);
                target.Hurt(damage, DamageType.Com15, _attacker.Nickname);
                target.ShowHint("<color=red>You are exposed to very serious radiation hydrochloride!</color>",10);
                _attacker.ShowHitMarker();

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject,4.5f);
            }
        }
    }
}