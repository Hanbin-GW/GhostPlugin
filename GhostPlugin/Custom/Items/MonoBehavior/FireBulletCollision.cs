using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerStatsSystem;
using UnityEngine;
using PlayerStatsSystem;


namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class FireBulletCollision : MonoBehaviour
    {
        private int _damage;
        private Player _attacker;

        public void Initialize(int damage, Player attacker)
        {
            _damage = damage;
            _attacker = attacker;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());

            if (target != null && target != _attacker)
            {
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                Log.Debug($"Hit Player: {target.Nickname} - Damage: {_damage}");
                target.EnableEffect<Burned>(duration: 5);
                //target.Hurt(_damage, DamageType.E11Sr, _attacker.Nickname);
                target.Hurt(new CustomReasonDamageHandler( "불탄 총알", _damage));
                _attacker.ShowHitMarker();
                Destroy(gameObject, 3f);
            }
            else
            {
                Destroy(gameObject,2f);
            }
        }
    }
}