using UnityEngine;
using Utilities;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        public NetworkVariableBool networkLeftClicked = new NetworkVariableBool(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        [SerializeField] private float forwardMoveSpeed;
        [SerializeField] private float sideWayMoveSpeed;

        private Rigidbody _rigidBody;
        private Camera _mainCamera;

        private Vector3 _moveInput;
        private Vector3 _moveVelocity;

        private Animator _animator;
        private static readonly int RunningFwd = Animator.StringToHash("runningFwd");
        private static readonly int RunningSideWay = Animator.StringToHash("runningSideWay");

        // Start is called before the first frame update
        public override void NetworkStart()
        {
            networkLeftClicked.Value = false;
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _mainCamera = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsLocalPlayer)
            {
                var horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
                var verticalAxisRaw = Input.GetAxisRaw("Vertical");
                var leftClicked = Input.GetButton("Fire1");

                AnimationControl(horizontalAxisRaw, verticalAxisRaw);

                MovementControl(horizontalAxisRaw, verticalAxisRaw);

                RotationControl();
                
                if (IsServer)
                {
                    networkLeftClicked.Value = leftClicked;
                }
                else
                {
                    ShootingControlServerRpc(leftClicked);
                }
            }
            
            _animator.SetLayerWeight(1, networkLeftClicked.Value ? 1 : 0);
        }

        [ServerRpc]
        private void ShootingControlServerRpc(bool leftClicked)
        {
            networkLeftClicked.Value = leftClicked;
        }

        private void RotationControl()
        {
            CameraUtils.LookAtMouseCursor(transform);
        }

        private void FixedUpdate()
        {
            _rigidBody.velocity = _moveVelocity;
        }

        public void MovementControl(float horizontalAxisRaw, float verticalAxisRaw)
        {
            if (horizontalAxisRaw == 0 && verticalAxisRaw == 0)
            {
                _moveVelocity = Vector3.zero;
                return;
            }

            _moveInput = new Vector3(horizontalAxisRaw, 0f, verticalAxisRaw);
            _moveVelocity = _moveInput * forwardMoveSpeed;

            // if (verticalAxisRaw == 0)
            // {
            //     _moveInput = new Vector3(horizontalAxisRaw, 0f, 0f);
            //     _moveVelocity = _moveInput * sideWayMoveSpeed;
            // }
            // else if (horizontalAxisRaw == 0)
            // {
            //     _moveInput = new Vector3(0f, 0f, verticalAxisRaw);
            //     _moveVelocity = _moveInput * forwardMoveSpeed;
            // }
            // else
            // {
            //     _moveInput = new Vector3(horizontalAxisRaw, 0f, verticalAxisRaw);
            //     _moveVelocity = new Vector3(_moveInput.x * sideWayMoveSpeed, 0f, _moveInput.z * forwardMoveSpeed);
            // }
        }

        private void AnimationControl(float horizontalAxisRaw, float verticalAxisRaw)
        {
            if (horizontalAxisRaw == 0 && verticalAxisRaw == 0)
            {
                _animator.SetInteger(RunningFwd, 0);
                _animator.SetInteger(RunningSideWay, 0);
            }
            else if (verticalAxisRaw == 0)
            {
                if (horizontalAxisRaw > 0)
                {
                    _animator.SetInteger(RunningFwd, 0);
                    _animator.SetInteger(RunningSideWay, 1);
                }
                else
                {
                    _animator.SetInteger(RunningFwd, 0);
                    _animator.SetInteger(RunningSideWay, -1);
                }
            }
            else if (verticalAxisRaw > 0)
            {
                _animator.SetInteger(RunningFwd, 1);
                _animator.SetInteger(RunningSideWay, 0);
            }
            else if (verticalAxisRaw < 0)
            {
                _animator.SetInteger(RunningFwd, -1);
                _animator.SetInteger(RunningSideWay, 0);
            }
        }
    }
}