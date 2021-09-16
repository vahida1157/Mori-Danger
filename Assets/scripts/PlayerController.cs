using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody _rigidBody;

    private Vector3 _moveInput;
    private Vector3 _moveVelocity;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
        var verticalAxisRaw = Input.GetAxisRaw("Vertical");
        
        AnimationControl(horizontalAxisRaw, verticalAxisRaw);
        
        _moveInput = new Vector3(horizontalAxisRaw, 0f, verticalAxisRaw);
        _moveVelocity = _moveInput * moveSpeed;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _moveVelocity;
    }

    private void AnimationControl(float horizontalAxisRaw, float verticalAxisRaw)
    {
        if (horizontalAxisRaw == 0 && verticalAxisRaw == 0)
        {
            _animator.SetInteger("runningFwd", 0);
            _animator.SetInteger("runningSideway", 0);
        } else if (verticalAxisRaw == 0){
            if (horizontalAxisRaw > 0)
            {
                _animator.SetInteger("runningFwd", 0);
                _animator.SetInteger("runningSideway", 1);
            }
            else
            {
                _animator.SetInteger("runningFwd", 0);
                _animator.SetInteger("runningSideway", -1);
            }
        } else if (verticalAxisRaw > 0) {
            _animator.SetInteger("runningFwd", 1);
            _animator.SetInteger("runningSideway", 0);
        } else if (verticalAxisRaw < 0) {
            _animator.SetInteger("runningFwd", -1);
            _animator.SetInteger("runningSideway", 0);
        }
    }
}