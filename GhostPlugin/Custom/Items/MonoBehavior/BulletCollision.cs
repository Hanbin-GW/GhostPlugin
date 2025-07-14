using Exiled.API.Enums;
using Exiled.API.Features;
using GhostPlugin.Methods.Objects;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class BulletCollision : MonoBehaviour
    {
        private int _damage;
        private Player _attacker;
        private Color _color;

        public void Initialize(int damage, Player attacker)
        {
            _damage = damage;
            _attacker = attacker;
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());

            if (target != null && target != _attacker)
            {
                if (target.Role.Team == _attacker.Role.Team)
                    return;
                Log.Debug($"Hit Player: {target.Nickname} - Damage: {_damage}");

                target.Hurt(_damage, DamageType.E11Sr, _attacker.Nickname);
                _attacker.ShowHitMarker();

                Destroy(gameObject, 7f);
            }
            else
            {
                Destroy(gameObject,4f);
            }
        }*/
        private void OnCollisionEnter(Collision collision)
        {
            var hitGameObject = collision.collider.gameObject;
            var hub = ReferenceHub.GetHub(hitGameObject);

            if (hub == null)
            {
                Destroy(gameObject, 4f);
                return;
            }

            Player target = Player.Get(hub);

            if (target != null && target != _attacker)
            {
                if (target.Role.Team == _attacker.Role.Team)
                    return;

                Log.Debug($"Hit Player: {target.Nickname} - Damage: {_damage}");

                target.Hurt(_damage, DamageType.E11Sr, _attacker.Nickname);
                _attacker.ShowHitMarker();
                switch (_attacker.Role.Team)
                {
                    case (Team.FoundationForces):
                        _color = new Color(0f, 1f, 1f, 0.1f) * 50; ;
                        break;
                    case (Team.Scientists):
                        _color = new Color(1f, 1f, 0f, 0.1f) * 50;
                        break;
                    case (Team.ChaosInsurgency):
                        _color = new Color(0.1f, 1f, 0.1f, 0.1f) * 50;
                        break;
                    case (Team.OtherAlive):
                        _color = new Color(1f, 1f, 1f, 0.1f) * 50;
                        break;
                }
                SpawnPrimitiveToy.Spawn(target, 5, _color);
                Destroy(gameObject, 1f);
            }
            else
            {
                Destroy(gameObject, 2.5f);
            }
        }

    }
}