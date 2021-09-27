using MLAPI;
using Player;
using UnityEngine;

namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] public float bulletDamage;
        private void OnCollisionEnter(Collision other)
        {
            // Debug.Log("collided");
            // var rigidBody = other.collider.GetComponentInParent<Rigidbody>();
            // if (rigidBody.tag.Equals("Player"))
            // {
            //     rigidBody.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            // }
            //
            // GetComponent<NetworkObject>().Despawn(true);
        }
    }
}