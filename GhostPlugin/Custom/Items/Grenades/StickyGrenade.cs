using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeHE)]
    public class StickyGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 30;
        public override string Name { get; set; } = "Sticky Grenade";
        public override string Description { get; set; } = "<color=red>Reminder: When it comes into contact with a watercolor, it sticks to it as it is</color> ";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool ExplodeOnCollision { get; set; } = false;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public override float FuseTime { get; set; } = 3.5f;

        protected override void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.Projectile.GameObject.AddComponent<StickyGrenadeBehavior>();
            }
        }

        protected override void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            base.UnsubscribeEvents();
        }


        private class StickyGrenadeBehavior : MonoBehaviour
        {
            private Rigidbody rb;
            private bool hasCollided = false;
        
            private void Awake()
            {
                rb = GetComponent<Rigidbody>();
            }

            private void OnCollisionEnter(Collision collision)
            {
                if (!hasCollided)
                {
                    hasCollided = true;

                    rb.isKinematic = true;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                
                    transform.position += transform.forward * 0.1f;
                }
            }
        }
    }
}