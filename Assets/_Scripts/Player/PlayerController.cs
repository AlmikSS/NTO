using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private bool _grounded;
    private Rigidbody2D _rb;
    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Jump.performed += context => Jump();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        SpeedControl();
    }

    private void Move()
    {
        float _axis = _playerInput.Player.Move.ReadValue<float>();
        Vector2 _direction = new Vector2(_axis, 0f);

        _rb.AddForce(_direction * _moveSpeed, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        Vector2 _flatVel = new Vector2(_rb.velocity.x, 0f);

        if (_flatVel.magnitude > _moveSpeed)
        {
            Vector2 _limitedVel = _flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector2(_limitedVel.x, _rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (_grounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D() => _grounded = true;

    private void OnEnable() => _playerInput.Enable();
  
    private void OnDisable() => _playerInput.Disable();
}