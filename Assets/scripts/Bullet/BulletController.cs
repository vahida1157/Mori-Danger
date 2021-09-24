using MLAPI;
using Player;
using UnityEngine;

namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float bulletDamage;
        private void OnCollisionEnter(Collision other)
        {
            var rigidBody = other.collider.GetComponentInParent<Rigidbody>();
            if (rigidBody.tag.Equals("Player"))
            {
                rigidBody.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            }

            Destroy(gameObject);
        }
    }
}