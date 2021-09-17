using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private const float MAXHealth = 300;
        private float _currentHealth;
        private bool _isDead;

        private void Start()
        {
            _currentHealth = MAXHealth;
            _isDead = false;
        }

        public void TakeDamage(float damageCount)
        {
            _currentHealth -= damageCount;

            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                Destroy(gameObject);
            }
        }
    }
}