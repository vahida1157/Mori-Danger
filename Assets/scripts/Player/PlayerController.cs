using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        public NetworkVariableBool networkLeftClicked = new NetworkVariableBool(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        [SerializeField] private float forwardMoveSpeed;
        [SerializeField] private float sideWayMoveSpeed;

        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform helperPoint;
        private Rigidbody _rigidBody;

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
            CameraUtils.PlayerLookAtMouseCursor(transform);
            // AddOffsetRotation();
        }

        private void AddOffsetRotation()
        {
            var position = transform.position;
            var firePointPosition = firePoint.position;
            var localMousePosition = CameraUtils.getInGameMousePosition(new Plane(Vector3.up, firePointPosition));

            var distance = Vector3.Distance(new Vector3(position.x, firePointPosition.y, position.z),
                new Vector3(localMousePosition.x, firePointPosition.y, localMousePosition.z));
            var height = helperPoint.localPosition.z - transform.localPosition.z;
            var wPosition = transform.TransformPoint(position.x, position.y, height);
            var angle = Mathf.Atan(height / distance);
            Debug.Log("height : " + height + "\tdistance : " + distance + "\tdegree : " + angle);
            Debug.DrawLine(new Vector3(position.x, firePointPosition.y, position.z),
                new Vector3(localMousePosition.x, firePointPosition.y, localMousePosition.z), Color.red);
            Debug.DrawLine(helperPoint.position,
                firePointPosition, Color.green);
            if (Input.GetKey("space"))
            {
                gameObject.transform.Rotate(Vector3.up, angle);
            }
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