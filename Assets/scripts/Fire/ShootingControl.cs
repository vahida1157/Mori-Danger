using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Utilities;

namespace Fire
{
    public class ShootingControl : NetworkBehaviour
    {
        [SerializeField] private float bulletForce;
        [SerializeField] private float shootingThreshold = 0.2f;

        private float _shotCounter;
        public Transform firePoint;

        public GameObject bulletPrefab;

        // Update is called once per frame
        void Update()
        {
            if (IsLocalPlayer)
            {
                // CameraUtils.LookAtMouseCursor(firePoint.transform, firePoint.position);

                if (Input.GetButton("Fire1"))
                {
                    _shotCounter -= Time.deltaTime;
                    if (_shotCounter <= 0)
                    {
                        _shotCounter = shootingThreshold;
                        Shoot();
                    }
                }
                else
                {
                    _shotCounter = 0;
                }
            }
        }

        private void Shoot()
        {
            if (IsServer)
            {
                var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<NetworkObject>().Spawn();
                var bulletRigidBody = bullet.GetComponent<Rigidbody>();
                bulletRigidBody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
            }
            else
            {
                SpawnBulletServerRpc();
            }
        }
        
        [ServerRpc] 
        public void SpawnBulletServerRpc()
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<NetworkObject>().Spawn();
            var bulletRigidBody = bullet.GetComponent<Rigidbody>();
            bulletRigidBody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }
    }
}