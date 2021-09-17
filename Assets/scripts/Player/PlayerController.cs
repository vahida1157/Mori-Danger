using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
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
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _mainCamera = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            var horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
            var verticalAxisRaw = Input.GetAxisRaw("Vertical");
            var leftClicked = Input.GetButton("Fire1");

            AnimationControl(horizontalAxisRaw, verticalAxisRaw);

            MovementControl(horizontalAxisRaw, verticalAxisRaw);

            RotationControl();

            ShootingControl(leftClicked);
        }

        private void ShootingControl(bool leftClicked)
        {
            _animator.SetLayerWeight(1, leftClicked ? 1 : 0);
        }

        private void RotationControl()
        {
            var cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(cameraRay, out var rayLength))
            {
                var pointToLock = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLock.x, transform.position.y, pointToLock.z));
            }
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
        
            if (verticalAxisRaw == 0)
            {
                _moveInput = new Vector3(horizontalAxisRaw, 0f, 0f);
                _moveVelocity = _moveInput * sideWayMoveSpeed;
            }
            else if (horizontalAxisRaw == 0)
            {
                _moveInput = new Vector3(0f, 0f, verticalAxisRaw);
                _moveVelocity = _moveInput * forwardMoveSpeed;
            }
            else
            {
                _moveInput = new Vector3(horizontalAxisRaw, 0f, verticalAxisRaw);
                _moveVelocity = new Vector3(_moveInput.x * sideWayMoveSpeed, 0f, _moveInput.z * forwardMoveSpeed);
            }
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