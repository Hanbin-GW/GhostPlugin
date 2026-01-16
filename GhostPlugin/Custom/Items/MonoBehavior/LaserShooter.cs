using AdminToys;
using Exiled.API.Enums;
using UnityEngine;
using Exiled.API.Features;
using Exiled.API.Features.Toys;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class LaserShooter : MonoBehaviour
    {
        public Player Owner;
        public float Damage = 50f;
        public float Range = 100f;

        public void FireLaser()
        {
            var origin = Owner.CameraTransform.position;
            var direction = Owner.CameraTransform.forward;

            RaycastHit[] hits = Physics.RaycastAll(origin, direction, Range);

            foreach (var hit in hits)
            {
                var target = Player.Get(hit.collider.gameObject);
                if (target != null && target != Owner)
                {
                    target.Hurt(Damage, DamageType.Custom, "Laser Beam");
                    Log.Debug($"Laser hit: {target.Nickname}");
                }
            }

            // Optional: Visual laser
            CreateLaserVisual(origin, direction);
        }

        private void CreateLaserVisual(Vector3 origin, Vector3 direction)
        {
            float length = Range;
            var color = Color.red * 50;
            var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            var position = origin + direction * 0.5f * length;
            var scale = new Vector3(0.2f, 0.5f * length, 0.2f);

            var laser = Primitive.Create(PrimitiveType.Cylinder, PrimitiveFlags.Visible, position, rotation.eulerAngles, scale, true, color);
            Destroy(laser.GameObject, 0.5f); // Destroy after 0.5 seconds
        }
    }

}