using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using GhostPlugin.Methods.Objects;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;


namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class FireBulletCollision : MonoBehaviour
    {
        private int _damage;
        private Player _attacker;
        private bool _hasCollided = false;
        private Color _color;
        public void Initialize(int damage, Player attacker)
        {
            _damage = damage;
            _attacker = attacker;
            Log.Debug($"[FireBullet] Initialized with damage: {damage}, attacker: {_attacker?.Nickname}");
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
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                Log.Debug($"Hit Player: {target.Nickname} - Damage: {_damage}");
                target.EnableEffect<Burned>(duration: 5);
                //target.Hurt(_damage, DamageType.E11Sr, _attacker.Nickname);
                target.Hurt(new CustomReasonDamageHandler( "불탄 총알", _damage));
                _attacker.ShowHitMarker();
                switch (_attacker.Role.Team)
                {
                    case (Team.FoundationForces):
                        _color = new Color(0f, 1f, 1f, 0.1f) * 50; ;
                        break;
                    case (Team.Scientists):
                        _color = new Color(1f, 1f, 0f, 0.1f) * 50;
                        break;
                    case (Team.OtherAlive):
                        _color = new Color(1f, 1f, 1f, 0.1f) * 50;
                        break;
                }
                SpawnPrimitiveToy.Spawn(target, 2, _color);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject,3f);
            }
        }
    }
}