using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerStatsSystem;
using UnityEngine;


namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class FireBulletCollision : MonoBehaviour
    {
        private int _damage;
        private Player _attacker;
        private bool _hasCollided = false;
        public void Initialize(int damage, Player attacker)
        {
            _damage = damage;
            _attacker = attacker;
            Log.Debug($"[FireBullet] Initialized with damage: {damage}, attacker: {_attacker?.Nickname}");
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());

            if (target != null && target != _attacker)
            {
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                _hasCollided = true; 
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                Log.Debug($"Hit Player: {target.Nickname} - Damage: {_damage}");
                target.EnableEffect<Burned>(duration: 5);
                //target.Hurt(_damage, DamageType.E11Sr, _attacker.Nickname);
                target.Hurt(new CustomReasonDamageHandler( "Buring bullet", _damage));
                _attacker.ShowHitMarker();
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject,3f);
            }
        }
    }
}