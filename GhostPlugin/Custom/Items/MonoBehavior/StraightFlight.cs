using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    [DisallowMultipleComponent]
    public class StraightFlight : MonoBehaviour
    {
        bool _initialized;
        Rigidbody _rb;
        Vector3 _dir;
        float _speed;
        bool _lockHorizontal;

        public void Init(Rigidbody rb, Vector3 dir, float speed, bool lockHorizontal)
        {
            if (_initialized) return;                 // ✅ 두 번째부터는 무시
            _initialized = true;

            _rb = rb;
            _dir = dir.normalized;
            _speed = speed;
            _lockHorizontal = lockHorizontal;

            // (선택) 혹시라도 중복 컴포넌트가 붙었으면 정리
            var dups = GetComponents<StraightFlight>();
            foreach (var dup in dups)
                if (dup != this) Destroy(dup);
        }

        void FixedUpdate()
        {
            if (_rb == null) return;

            if (_lockHorizontal) { _dir.y = 0f; _dir.Normalize(); }

            _rb.useGravity = false;
            _rb.velocity = _dir * _speed;
            transform.forward = _dir;
        }
    }
}