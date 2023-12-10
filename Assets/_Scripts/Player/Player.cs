using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField] private Transform _attackPosition; // поле позиции нанесения урона
    [SerializeField] private LayerMask _attackMask; // слой, которому наносим урон
    [SerializeField] private float _attackRadius; // радиус аттаки
    [SerializeField] private int _damage; // урон
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;

    [Header("General")]
    [SerializeField] private float _maxHealth; // максимальное здоровье
    [SerializeField] private GameObject _inventory; // поле инвентаря
    [SerializeField] private Inventory _inv;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private GameObject _deathScreen;
    private Animator _animator; // поле Animator
    private float _health; // здоровье

    [Header("Audio")]
    [SerializeField] private AudioSource _hertAudio;
    [SerializeField] private AudioSource _rangedAttackAudio;

    [Header("Gadjets")]
    [SerializeField] private GadjetsAbilitys _abilities;
    [SerializeField] private Transform _rangedPoint;
    public bool CanTakeDamage = true;
    private Input _playerInput; // ввод игрока

    public void Awake()
    {
        _playerInput = new Input(); // создаем экземпляр класса Input 
        _playerInput.Player.MouseLeftButtonClick.performed += Attack; // подписываем метод Attack к событию нажатия кнопки атаки
        _playerInput.Player.ShowInventory.performed += context => ShowCloseInventory(); // подписываем метод ShowCloseInventory к событию нажатия кнопки инвентаря
        _playerInput.Player.MouseRightButtonClick.performed += context => RangedAttack();
    }

    private void Start()
    {
        Time.timeScale = 1;
        _healthBar.maxValue = _maxHealth;
        _health = _maxHealth; // текущее здоровье равно максимальному
        _animator = GetComponent<Animator>(); // кэшируем Animator
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (_health >= 0)
        {
            _health -= Time.deltaTime;
            _healthBar.value = _health;
            yield return null;
        }

        Die();
    }

    public void Attack(InputAction.CallbackContext context) // метод атаки
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

    private void RangedAttack()
    {
        _animator.SetTrigger("RangedAttack");
    }

    public void Shoot()
    {
        GameObject obj = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().velocity = (Vector2.right * transform.localScale.x) * _bulletSpeed;
        _rangedAttackAudio.Play();
    }

    private void MakeDamage(int damage) // ����� ��������� �����
    {
        // �������� ��� ���������� � ���� �����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPosition.position, _attackRadius, _attackMask);
        // ���������� �� ���� �����������
        foreach (Collider2D enemy in colliders)
        {
            try
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            } // ������� ����
            catch (Exception)
            {
                enemy.GetComponent<ShootingEnemy>().TakeDamage(damage);
            }
        }
    }

    private void ShowCloseInventory() // ����� �������� � ��������� ���������
    {
        _inventory.SetActive(!_inventory.activeSelf); // включаем/выключаем инвентарь

        if (_inventory.activeSelf) // если инвентарь включен
            Time.timeScale = 0f; // останавливаем время
        else if (!_inventory.activeSelf) // если инвентарь выключен
            Time.timeScale = 1f; // возобновляем время
    }

    public void TakeDamage(int damage) // метод получения урона
    {
        if (!CanTakeDamage)
            return;
        if (damage > 0) // если урон больше 0
            _health -= damage; // наносим урон

        _hertAudio.Play();

        if (_health <= 0) // если здоровье меньше или равно нулю
            Die(); // умираем
    }

    private void Die() // метод смерти
    {
        _deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnApplicationQuit()
    {
        _inv.Save();
        MainMenuManager.Save("Scene");
    }

    private void OnEnable() => _playerInput.Enable(); // включаем систему ввода

    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}