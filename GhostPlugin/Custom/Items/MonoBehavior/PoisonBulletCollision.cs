using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class PoisonBulletCollision : MonoBehaviour
    {
        //private int _damage;
        private Player _attacker;

        public void Initialize(Player attacker)
        {
            //_damage = damage;
            _attacker = attacker;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player target = Player.Get(collision.collider) ?? Player.Get(collision.collider.GetComponentInParent<Collider>());

            if (target != null && target != _attacker && !(_attacker.LeadingTeam == target.LeadingTeam))
            {
                Log.Debug($"Hit Player: {target.Nickname}");
                
                target.EnableEffect<Decontaminating>(duration: 8,intensity:1);
                target.EnableEffect<Poisoned>(duration: 20,intensity:2);
                target.Hurt(5, DamageType.Com15, _attacker.Nickname);
                target.ShowHint("<color=red>매우 심각한 방사선 염산물질에 노출되었습니다!</color>",10);
                _attacker.ShowHitMarker();

                Destroy(gameObject, 4.5f);
            }
            else
            {
                Destroy(gameObject,0.1f);
            }
        }
    }
}