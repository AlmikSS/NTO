using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IDamager
{
    private const float DOUBLECLICKTIME = 1f; // константа времени двойного клика
    [Header("Attack")]
    [SerializeField] private Transform _attackPosition; // поле позиции нанесени€ урона
    [SerializeField] private LayerMask _attackMask; // слой, которому наносим урон
    [SerializeField] private float _attackRadius; // радиус аттаки
    [SerializeField] private int _damage; // урон
    private float _lastClickTime; // врем€ последнего клика

    [Header("General")]
    [SerializeField] private int _maxHealth; // максимальное здоровье
    [SerializeField] private GameObject _inventory; // поле инвентар€
    private Animator _animator; // поле Animator
    private int _health; // здоровье

    [Header("Gadjets")]
    [SerializeField] private GadjetsAbilitys _abilities;

    private Input _playerInput; // ввод игрока

    public void Awake()
    {
        _playerInput = new Input(); // создаем экземпл€р класса Input 
        _playerInput.Player.MouseLeftButtonClick.performed += context => Attack(); // подписываем метод Attack к событию нажати€ кнопки атаки
        _playerInput.Player.ShowInventory.performed += context => ShowCloseInventory(); // подписываем метод ShowCloseInventory к событию нажати€ кнопки инвентар€
    }

    private void Start()
    {
        Load();
        _health = _maxHealth; // текущее здоровье равно максимальному
        _animator = GetComponent<Animator>(); // кэшируем Animator
    }

    private void Load()
    {
        int posX = PlayerPrefs.GetInt("PosX");
        int posY = PlayerPrefs.GetInt("PosY");

        transform.position = new Vector3(posX, posY, 0);
    }

    public void Attack() // метод атаки
    {
        if (Time.time - _lastClickTime > DOUBLECLICKTIME)
        {
            MakeDamage(_damage); // наносим урон
            _animator.SetTrigger("Attack"); // проигрывем анимацию
        }

        if (Time.time - _lastClickTime < DOUBLECLICKTIME)
        {
            MakeDamage(_damage + 2); // наносим увеличенный урон
            _animator.Play("Combo"); // анимаци€ комбо удара
        }

        _lastClickTime = Time.time; // записываем врем€ последнего клика
    }

    private void MakeDamage(int damage) // метод нанесени€ урона
    {
        // собираем все коллайдеры в зоне атаки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPosition.position.x, _attackPosition.position.y), _attackRadius, _attackMask);
        // проходимс€ по всем коллайдерам
        foreach (Collider2D enemy in colliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage); // наносим урон
        }
    }

    private void ShowCloseInventory() // метод открыти€ и закртыти€ инвентар€
    {
        _inventory.SetActive(!_inventory.activeSelf); // включаем/выключаем инвентарь

        if (_inventory.activeSelf) // если инвентарь включен
            Time.timeScale = 0f; // останавливаем врем€
        else if (!_inventory.activeSelf) // если инвентарь выключен
            Time.timeScale = 1f; // возобновл€ем врем€
    }

    public void TakeDamage(int damage) // метод получени€ урона
    {
        if (damage > 0) // если урон больше 0
            _health -= damage; // наносим урон

        if (_health <= 0) // если здоровье меньше или равно нулю
            Die(); // умираем
    }

    private void Die() // метод смерти
    {
        Destroy(gameObject); // удаление этого объекта
    }

    private void OnEnable() => _playerInput.Enable(); // включаем систему ввода

    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}