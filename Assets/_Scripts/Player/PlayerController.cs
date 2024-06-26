using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _slidingSpeed;
    [SerializeField] private float _wallCheckDistance;

    [Header("Jumping")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundedMask;
    [SerializeField] private float _jumpForce;
    [SerializeField] private AudioSource _jumpAudio;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Input _playerInput;
    private bool _isSliding;
    private bool _isTouchingWall;
    private bool _isFacingRight = true;
    private float _moveDirection;

    public bool Grounded;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Jump.performed += context => Jump(Grounded);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Grounded = Physics2D.OverlapCircle(_groundCheck.position, 0.3f, _groundedMask);
        _moveDirection = _playerInput.Player.Move.ReadValue<float>();
        if (Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, _groundedMask) || Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, _groundedMask))
            _isTouchingWall = true;
        else
            _isTouchingWall = false;
        Flip();
        ApplyAnimations();
        CheckSliding();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckSliding()
    {
        if (_isTouchingWall && !Grounded)
            _isSliding = true;
        else
            _isSliding = false;
    }

    private void ApplyAnimations()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_moveDirection));
        _animator.SetBool("Grounded", Grounded);
    }

    public void Jump(bool ready)
    {
        if (ready)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpAudio.Play();
        }
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_movementSpeed * _moveDirection, _rb.velocity.y);

        if (_isSliding)
        {
            if (_rb.velocity.y < -_slidingSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_slidingSpeed);
            }
        }
    }

    private void Flip()
    {
        if (_isFacingRight && _moveDirection < 0 || !_isFacingRight && _moveDirection > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}