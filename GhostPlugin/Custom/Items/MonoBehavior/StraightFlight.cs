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

            // 혹시 모를 외력/중력/스크립트 변경을 덮어쓰기
            if (_lockHorizontal) {
                _dir.y = 0f;             // 항상 수평 유지
                _dir.Normalize();
            }

            _rb.useGravity = false;      // 어떤 스크립트가 켜도 다시 끔
            _rb.velocity = _dir * _speed;
            transform.forward = _dir;    // 시각적으로도 정렬
        }
    }
}