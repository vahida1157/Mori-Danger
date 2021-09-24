using System;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        private const float MAXHealth = 300;
        private float _currentHealth;
        private NetworkVariableBool _isDead = new NetworkVariableBool(new NetworkVariableSettings()
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.Everyone,
        });
        private NetworkVariableBool _gotShot = new NetworkVariableBool(new NetworkVariableSettings()
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.Everyone,
        });
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int GotShot = Animator.StringToHash("gotShot");

        private Animator _animator;
        
        public override void NetworkStart()
        {
            _currentHealth = MAXHealth;
            _gotShot.Value = false;
            _animator = GetComponent<Animator>();
        }

        public void SetShotToFalse()
        {
            _gotShot.Value = false;
            _animator.SetBool(GotShot, _gotShot.Value);
        }

        public void TakeDamage(float damageCount)
        {
            Debug.Log("took damage, isDead: " + _isDead.Value);
            _currentHealth -= damageCount;

            if (_currentHealth <= 0 && !_isDead.Value)
            {
                _isDead.Value = true;
                _animator.SetBool(IsDead, _isDead.Value);
                // Destroy(gameObject);
            }
            if (!_isDead.Value)
            {
                _gotShot.Value = true;
                _animator.SetBool(GotShot, _gotShot.Value);
            }
            Debug.Log("took damage 2, isDead: " + _isDead.Value);
        }

        public bool IsPlayerDead()
        {
            return _isDead.Value;
        }
    }
}