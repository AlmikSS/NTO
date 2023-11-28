using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed; // скорость передвижения
    [SerializeField] private float _jumpForce; // сила прыжка
    [SerializeField] private LayerMask _groundMask;

    [Header("General")]
    private Vector3 _scale; // поле размера игрока
    private bool _grounded; // логическая переменная показывающая находимся мы на земле или нет
    private Rigidbody2D _rb; // поле Rigidbody2D для физических взаимодействий
    private Input _playerInput; // система ввода
    private Animator _animator; // поле Animator

    public void Awake()
    {
        _playerInput = new Input(); // создаем экземпляр класса Input
        _playerInput.Player.Jump.performed += context => Jump(); // подписываем метод Jump к событию нажатия на кнопку прыжка
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // кэшируем Rigidbody2D
        _scale = transform.localScale; // кэшируем размер игрока
        _animator = GetComponent<Animator>(); // кэшируем Animator
    }

    private void Update()
    {
        _grounded = Physics2D.Raycast(transform.position ,Vector2.down, _scale.y / 2, _groundMask); // на земле ли мы

        SpeedControl(); // вызываем метод SpeedControl

        // включаем анимации
        _animator.SetFloat("Speed", Mathf.Abs(_rb.velocity.x));
        _animator.SetBool("Grounded", _grounded);
    }

    private void FixedUpdate()
    {
        Move(); // в FixedUpdate вызываем метод Move      
    }

    private void Move() // метод перемещения игрока
    {
        float _axis = _playerInput.Player.Move.ReadValue<float>(); // определяем направление движения 
        _rb.velocity = new Vector2(_axis * _moveSpeed, _rb.velocity.y); // двигаемся

        if (_axis > 0) // двигаемся вправо
            transform.localScale = _scale; // смотрим вправо
        else if (_axis < 0) // двигаемся влево
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z); // смотрим влево
    }

    private void SpeedControl() // метод контроля скорости
    {
        Vector2 _flatVel = new Vector2(_rb.velocity.x, 0f); // вектор текущей скорости

        if (_flatVel.magnitude > _moveSpeed) // текущая скорость больше скорости передвижения
        {
            Vector2 _limitedVel = _flatVel.normalized * _moveSpeed; // вектор с нормализованной скоростью
            _rb.velocity = new Vector2(_limitedVel.x, _rb.velocity.y); // задаем движение
        }
    }

    private void Jump() // метод прыжка
    {
        if (_grounded) // находимся на земле
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse); // прыгаем
        }
    }

    private void OnEnable() => _playerInput.Enable(); // включаем систему ввода
  
    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}