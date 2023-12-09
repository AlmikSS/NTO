using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField] private Transform _attackPosition; // ���� ������� ��������� �����
    [SerializeField] private LayerMask _attackMask; // ����, �������� ������� ����
    [SerializeField] private float _attackRadius; // ������ ������
    [SerializeField] private int _damage; // ����

    [Header("General")]
    [SerializeField] private int _maxHealth; // ������������ ��������
    [SerializeField] private GameObject _inventory; // ���� ���������
    private Animator _animator; // ���� Animator
    private int _health; // ��������

    [Header("Gadjets")]
    [SerializeField] private GadjetsAbilitys _abilities;

    private Input _playerInput; // ���� ������

    public void Awake()
    {
        _playerInput = new Input(); // ������� ��������� ������ Input 
        _playerInput.Player.MouseLeftButtonClick.performed += Attack; // ����������� ����� Attack � ������� ������� ������ �����
        _playerInput.Player.ShowInventory.performed += context => ShowCloseInventory(); // ����������� ����� ShowCloseInventory � ������� ������� ������ ���������
    }

    private void Start()
    {
        _health = _maxHealth; // ������� �������� ����� �������������
        _animator = GetComponent<Animator>(); // �������� Animator
    }


    public void Attack(InputAction.CallbackContext context) // ����� �����
    {
        if (context.interaction is TapInteraction)
        {
            MakeDamage(_damage); // ������� ����
            _animator.SetTrigger("Attack"); // ���������� ��������
        }

        if (context.interaction is MultiTapInteraction)
        {
            MakeDamage(_damage + 2); // ������� ����������� ����
            _animator.Play("Combo"); // �������� ����� �����
        }
    }

    private void MakeDamage(int damage) // ����� ��������� �����
    {
        // �������� ��� ���������� � ���� �����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPosition.position.x, _attackPosition.position.y), _attackRadius, _attackMask);
        // ���������� �� ���� �����������
        foreach (Collider2D enemy in colliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage); // ������� ����
        }
    }

    private void ShowCloseInventory() // ����� �������� � ��������� ���������
    {
        _inventory.SetActive(!_inventory.activeSelf); // ��������/��������� ���������

        if (_inventory.activeSelf) // ���� ��������� �������
            Time.timeScale = 0f; // ������������� �����
        else if (!_inventory.activeSelf) // ���� ��������� ��������
            Time.timeScale = 1f; // ������������ �����
    }

    public void TakeDamage(int damage) // ����� ��������� �����
    {
        if (damage > 0) // ���� ���� ������ 0
            _health -= damage; // ������� ����

        if (_health <= 0) // ���� �������� ������ ��� ����� ����
            Die(); // �������
    }

    private void Die() // ����� ������
    {
        Destroy(gameObject); // �������� ����� �������
    }

    private void OnEnable() => _playerInput.Enable(); // �������� ������� �����

    private void OnDisable() => _playerInput.Disable(); // ��������� ������� �����
}