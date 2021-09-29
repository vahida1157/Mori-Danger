using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Spawning;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        private const float MAXHealth = 300;
        public bool dead;

        private NetworkVariableFloat _currentHealth = new NetworkVariableFloat(new NetworkVariableSettings()
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly,            
        });
        
        public override void NetworkStart()
        {
            _currentHealth.Value = MAXHealth;
        }
        
        public void TakeDamage(float damageCount)
        {
            if (IsServer)
            {
                _currentHealth.Value -= damageCount;
            }
            else
            {
                TakeDamageServerRpc(damageCount);
            }
        }

        [ServerRpc]
        private void TakeDamageServerRpc(float damageCount)
        {
            _currentHealth.Value -= damageCount;
        }

        public float GetCurrentHealth()
        {
            return _currentHealth.Value;
        }

    }
}