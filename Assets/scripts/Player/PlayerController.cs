using Bullet;
using MLAPI;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float forwardMoveSpeed;

        private Rigidbody _rigidBody;
        private PlayerHealth _playerHealth;

        private Vector3 _moveVelocity;

        private Animator _animator;
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Shooting = Animator.StringToHash("Shooting");

        public override void NetworkStart()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            if (_playerHealth.GetCurrentHealth() > 0)
            {
                if (IsLocalPlayer)
                {
                    var horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
                    var verticalAxisRaw = Input.GetAxisRaw("Vertical");
                    var leftClicked = Input.GetButton("Fire1");

                    AnimationControl(horizontalAxisRaw, verticalAxisRaw, leftClicked);

                    MovementControl(horizontalAxisRaw, verticalAxisRaw);

                    RotationControl();
                }
            }
            else if (!_playerHealth.dead)
            {
                ResetValues();
                PlayerDeath();
            }
        }

        private void ResetValues()
        {
            _moveVelocity = Vector3.zero;
            _animator.SetBool(Moving, false);
            _animator.SetBool(Shooting, false);
        }

        private void PlayerDeath()
        {
            _playerHealth.dead = true;
            _animator.SetBool(Dead, true);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("collided");
            if (other.collider.tag.Equals("Bullet"))
            {
                _animator.SetTrigger(Hit);
                GetComponent<PlayerHealth>()
                    .TakeDamage(other.gameObject.GetComponent<BulletController>().bulletDamage);
            }
        }

        private void RotationControl()
        {
            CameraUtils.PlayerLookAtMouseCursor(transform);
        }


        private void FixedUpdate()
        {
            _rigidBody.velocity = _moveVelocity;
        }

        private void MovementControl(float horizontalAxisRaw, float verticalAxisRaw)
        {
            if (horizontalAxisRaw == 0 && verticalAxisRaw == 0)
            {
                _moveVelocity = Vector3.zero;
                return;
            }

            var moveInput = new Vector3(horizontalAxisRaw, 0f, verticalAxisRaw);
            _moveVelocity = moveInput * forwardMoveSpeed;
        }

        private void AnimationControl(float horizontalAxisRaw, float verticalAxisRaw, bool leftClicked)
        {
            _animator.SetBool(Moving, horizontalAxisRaw != 0f || verticalAxisRaw != 0f);
            if (horizontalAxisRaw != 0f || verticalAxisRaw != 0f)
            {
                _animator.SetFloat(Horizontal, horizontalAxisRaw);
                _animator.SetFloat(Vertical, verticalAxisRaw);
            }

            _animator.SetBool(Shooting, leftClicked);
        }
    }
}