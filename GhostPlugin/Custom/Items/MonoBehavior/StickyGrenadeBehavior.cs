using UnityEngine;

namespace GhostPlugin.Custom.Items.MonoBehavior
{
    public class StickyGrenadeBehavior : MonoBehaviour
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
                //rb.velocity = Vector3.zero;
                //rb.angularVelocity = Vector3.zero;
                
                transform.position += transform.forward * 0.0f;
            }
        }
    }
}