using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // �������� ������������
    [SerializeField] private float _jumpForce; // ���� ������

    private bool _grounded; // ���������� ���������� ������������ ��������� �� �� ����� ��� ���
    private Rigidbody2D _rb; // ���� Rigidbody2D ��� ���������� ��������������
    private Input _playerInput; // ������� �����

    private void Awake()
    {
        _playerInput = new Input(); // ������� ��������� ������ Input
        _playerInput.Player.Jump.performed += context => Jump(); // ����������� ����� Jump � ������� ������� �� ������ ������
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // �������� Rigidbody2D
    }

    private void FixedUpdate()
    {
        Move(); // � FixedUpdate �������� ����� Move
        SpeedControl(); // �������� ����� SpeedControl
    }

    private void Move() // ����� ����������� ������
    {
        float _axis = _playerInput.Player.Move.ReadValue<float>(); // ���������� ����������� �������� 

        _rb.velocity = new Vector2(_axis * _moveSpeed, _rb.velocity.y); // ���������
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

    private void Jump() // ����� ������
    {
        if (_grounded) // ��������� �� �����
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse); // �������
            _grounded = false; // ������ �� �� �����
        }
    }

    private void OnCollisionEnter2D() => _grounded = true; // ������������ �� �����

    private void OnEnable() => _playerInput.Enable(); // �������� ������� �����
  
    private void OnDisable() => _playerInput.Disable(); // ��������� ������� �����
}