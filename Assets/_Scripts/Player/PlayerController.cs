using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed; // �������� ������������
    [SerializeField] private float _jumpForce; // ���� ������
    [SerializeField] private LayerMask _groundMask;

    [Header("General")]
    private Vector3 _scale; // ���� ������� ������
    public bool Grounded; // ���������� ���������� ������������ ��������� �� �� ����� ��� ���
    private Rigidbody2D _rb; // ���� Rigidbody2D ��� ���������� ��������������
    private Input _playerInput; // ������� �����
    private Animator _animator; // ���� Animator

    public void Awake()
    {
        _playerInput = new Input(); // ������� ��������� ������ Input
        _playerInput.UI.Disable();
        _playerInput.Player.Jump.performed += context => Jump(Grounded); // ����������� ����� Jump � ������� ������� �� ������ ������
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // �������� Rigidbody2D
        _scale = transform.localScale; // �������� ������ ������
        _animator = GetComponent<Animator>(); // �������� Animator
    }

    private void Update()
    {
        Grounded = Physics2D.Raycast(transform.position ,Vector2.down, _scale.y / 2, _groundMask); // �� ����� �� ��

        SpeedControl(); // �������� ����� SpeedControl

        // �������� ��������
        _animator.SetFloat("Speed", Mathf.Abs(_rb.velocity.x));
        _animator.SetBool("Grounded", Grounded);
    }

    private void FixedUpdate()
    {
        Move(); // � FixedUpdate �������� ����� Move      
    }

    private void Move() // ����� ����������� ������
    {
        float _axis = _playerInput.Player.Move.ReadValue<float>(); // ���������� ����������� �������� 
        _rb.velocity = new Vector2(_axis * _moveSpeed, _rb.velocity.y); // ���������

        if (_axis > 0) // ��������� ������
            transform.localScale = _scale; // ������� ������
        else if (_axis < 0) // ��������� �����
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z); // ������� �����
    }

    private void SpeedControl() // ����� �������� ��������
    {
        Vector2 _flatVel = new Vector2(_rb.velocity.x, 0f); // ������ ������� ��������

        if (_flatVel.magnitude > _moveSpeed) // ������� �������� ������ �������� ������������
        {
            Vector2 _limitedVel = _flatVel.normalized * _moveSpeed; // ������ � ��������������� ���������
            _rb.velocity = new Vector2(_limitedVel.x, _rb.velocity.y); // ������ ��������
        }
    }

    public void Jump(bool ready) // ����� ������
    {
        if (ready) // ��������� �� �����
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse); // �������
        }
    }

    private void OnEnable() => _playerInput.Enable(); // �������� ������� �����
  
    private void OnDisable() => _playerInput.Disable(); // ��������� ������� �����
}