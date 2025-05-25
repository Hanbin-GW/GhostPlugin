using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using GhostPlugin.Custom.Items.MonoBehavior;
using UnityEngine;

namespace GhostPlugin.Methods.Objects
{
    public class SpawnPrimitive
    {
        public static void spawnPrimitive(Player player,PrimitiveType primitiveType,Quaternion rotation, Vector3 laserPos, Color laserColor,int damage)
        {
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
            Primitive pt = Primitive.Create(primitiveType,
                PrimitiveFlags.Visible | PrimitiveFlags.Collidable, laserPos,
                rotation.eulerAngles,
                scale, true, laserColor);
            var bulletcollision = pt.GameObject.AddComponent<BulletCollision>();
            bulletcollision.Initialize(damage, player);
            var rb = pt.GameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.mass = 1f;
            rb.drag = 0.5f;
            rb.angularDrag = 0.1f;
            rb.velocity = player.GameObject.transform.forward * 25;

            if (pt.GameObject.GetComponent<Collider>() == null)
                pt.GameObject.AddComponent<BoxCollider>();
        }
        public static void spawnPrimitives(Player player,int count,Quaternion rotation, Vector3 laserPos, Color laserColor,int damage, int velocity)
        {
            Vector3 scale = new Vector3(0.05f, 0.05f, 0.05f);
            for (int i = 0; i < count; i++)
            {
                Primitive pt = Primitive.Create(PrimitiveType.Sphere,
                    PrimitiveFlags.Visible | PrimitiveFlags.Collidable, laserPos,
                    rotation.eulerAngles,
                    scale, true, laserColor);
                var bulletcollision = pt.GameObject.AddComponent<FireBulletCollision>();
                bulletcollision.Initialize(damage, player);
                var rb = pt.GameObject.AddComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.mass = 1f;
                rb.drag = 0.5f;
                rb.angularDrag = 0.1f;
                rb.velocity = player.GameObject.transform.forward * velocity;

                if (pt.GameObject.GetComponent<Collider>() == null)
                    pt.GameObject.AddComponent<BoxCollider>();

            }
        }
        
        public static void spawnPrimitivesNoGravity(Player player,int count,Quaternion rotation, Vector3 laserPos, Color laserColor,int damage, int velocity)
        {
            Vector3 scale = new Vector3(0.05f, 0.05f, 0.05f);
            for (int i = 0; i < count; i++)
            {
                Primitive pt = Primitive.Create(PrimitiveType.Sphere,
                    PrimitiveFlags.Visible | PrimitiveFlags.Collidable, laserPos,
                    rotation.eulerAngles,
                    scale, true, laserColor);
                var bulletcollision = pt.GameObject.AddComponent<FireBulletCollision>();
                bulletcollision.Initialize(damage, player);
                var rb = pt.GameObject.AddComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.mass = 1f;
                rb.drag = 0.1f;
                rb.angularDrag = 0.1f;
                rb.velocity = player.CameraTransform.forward * velocity;
                if (pt.GameObject.GetComponent<Collider>() == null)
                    pt.GameObject.AddComponent<BoxCollider>();

            }
        }
    }
}