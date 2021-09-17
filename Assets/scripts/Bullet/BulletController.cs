using Player;
using UnityEngine;

namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var rigidBody = other.collider.GetComponentInParent<Rigidbody>();
            if (rigidBody.tag.Equals("Player"))
            {
                rigidBody.GetComponent<PlayerHealth>().TakeDamage(100);
            }
            Destroy(gameObject);
        }
    }
}