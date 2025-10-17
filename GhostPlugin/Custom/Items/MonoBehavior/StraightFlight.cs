using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class StraightFlight : MonoBehaviour
    {
        Rigidbody _rb;
        Vector3 _dir;
        float _speed;
        bool _lockHorizontal;

        public void Init(Rigidbody rb, Vector3 dir, float speed, bool lockHorizontal)
        {
            _rb = rb; _dir = dir.normalized; _speed = speed; _lockHorizontal = lockHorizontal;
        }

        void FixedUpdate()
        {
            if (_rb == null) return;

            if (_lockHorizontal)
            {
                _dir.y = 0f;
                _dir.Normalize();
            }

            _rb.useGravity = false;
            _rb.velocity = _dir * _speed;
            transform.forward = _dir;
        }
    }
}