using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IDamager
{
    private const float DOUBLECLICKTIME = 1f; // ��������� ������� �������� �����
    [Header("Attack")]
    [SerializeField] private Transform _attackPosition; // ���� ������� ��������� �����
    [SerializeField] private LayerMask _attackMask; // ����, �������� ������� ����
    [SerializeField] private float _attackRadius; // ������ ������
    [SerializeField] private int _damage; // ����
    private float _lastClickTime; // ����� ���������� �����

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
        _playerInput.Player.MouseLeftButtonClick.performed += context => Attack(); // ����������� ����� Attack � ������� ������� ������ �����
        _playerInput.Player.ShowInventory.performed += context => ShowCloseInventory(); // ����������� ����� ShowCloseInventory � ������� ������� ������ ���������
    }

    private void Start()
    {
        //Load();
        _health = _maxHealth; // ������� �������� ����� �������������
        _animator = GetComponent<Animator>(); // �������� Animator
    }

    private void Load()
    {
        int posX = PlayerPrefs.GetInt("PosX");
        int posY = PlayerPrefs.GetInt("PosY");

        PlayerPrefs.DeleteKey("PosX");
        PlayerPrefs.DeleteKey("PosY");
        transform.position = new Vector3(posX, posY, 0);
    }

    public void Attack() // ����� �����
    {
        if (Time.time - _lastClickTime > DOUBLECLICKTIME)
        {
            MakeDamage(_damage); // ������� ����
            _animator.SetTrigger("Attack"); // ���������� ��������
        }

        if (Time.time - _lastClickTime < DOUBLECLICKTIME)
        {
            MakeDamage(_damage + 2); // ������� ����������� ����
            _animator.Play("Combo"); // �������� ����� �����
        }

        _lastClickTime = Time.time; // ���������� ����� ���������� �����
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