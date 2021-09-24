using System;
using MLAPI;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        private const float MAXHealth = 300;
        private float _currentHealth;
        private bool _isDead = false;
        private bool _gotShot;
        private Animator _animator;
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int GotShot = Animator.StringToHash("gotShot");

        private void Start()
        {
            _currentHealth = MAXHealth;
            _gotShot = false;
            _animator = GetComponent<Animator>();
        }

        public void SetShotToFalse()
        {
            _gotShot = false;
            _animator.SetBool(GotShot, _gotShot);
        }

        public void TakeDamage(float damageCount)
        {
            _currentHealth -= damageCount;

            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                _animator.SetBool(IsDead, _isDead);
                // Destroy(gameObject);
            }
            else
            {
                _gotShot = true;
                _animator.SetBool(GotShot, _gotShot);
            }
        }

        public bool IsPlayerDead()
        {
            return _isDead;
        }
    }
}