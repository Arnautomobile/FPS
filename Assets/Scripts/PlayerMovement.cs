using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Collision Checking")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector3 _groundCheckDimensions;
    [SerializeField] private LayerMask _collisionLayer;

    [Header("Mouvement Parameters")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpHoldTime;

    private CharacterController _controller;
    private Vector3 _direction;
    private float _ySpeed = 0;
    private float _speed = 0;
    private float _jumpHoldTimer = 0;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _direction = Vector3.zero;
        _speed = _walkSpeed;
    }


    void Update()
    {
        Vector3 moveX = transform.right * Input.GetAxis("Horizontal");
        Vector3 moveZ = transform.forward * Input.GetAxis("Vertical");
        Vector3 direction = (moveX + moveZ).normalized;

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            _speed = _speed == _walkSpeed ? _runSpeed : _walkSpeed;
        }

        if (IsGrounded()) {
            if (_ySpeed < 0) {
                // Dont move down when ySpeed > 0 to not block the jump
                _ySpeed = -2f;
                _direction = direction;
            }

            if (Input.GetButtonDown("Jump")) {
                // Reset the values for new jump
                _jumpHoldTimer = 0;
                _ySpeed = _jumpForce;
            }
        }
        else {
            // Lerp the direction by the acceleration when in the air to make the movements less reactive
            _direction.x = Mathf.Lerp(_direction.x, direction.x, _airAcceleration * Time.deltaTime);
            _direction.z = Mathf.Lerp(_direction.z, direction.z, _airAcceleration * Time.deltaTime);

            if (_jumpHoldTimer < _jumpHoldTime) {
                _jumpHoldTimer += Time.deltaTime;

                if (Input.GetButton("Jump")) {
                    // In case the GetButtonUp happens before entering in this statement
                    _ySpeed += _jumpForce * Time.deltaTime;
                }
                else if (Input.GetButtonUp("Jump")) {
                    _jumpHoldTimer = _jumpHoldTime;
                }
            }
            _ySpeed -= _gravity * Time.deltaTime;
        }

        _controller.Move(_speed * Time.deltaTime * _direction + new Vector3(0, _ySpeed, 0));
    }


    private bool IsGrounded()
    {
        return Physics.OverlapBox(_groundCheckTransform.position, _groundCheckDimensions * 0.5f,
               Quaternion.identity, _collisionLayer).Length > 0;
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckDimensions);
    }
}
