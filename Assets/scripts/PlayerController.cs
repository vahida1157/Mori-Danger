using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody _rigidBody;
    private Camera _mainCamera;

    private Vector3 _moveInput;
    private Vector3 _moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();

        RotationControl();
    }

    private void RotationControl()
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out var rayLength))
        {
            Vector3 pointToLock = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLock.x, transform.position.y, pointToLock.z));
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _moveVelocity;
    }

    private void MovementControl()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            _moveVelocity = Vector3.zero;
            return;
        }

        _moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _moveVelocity = _moveInput * moveSpeed;
    }
}