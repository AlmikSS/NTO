using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // скорость передвижени€
    [SerializeField] private float _jumpForce; // сила прыжка

    private bool _grounded; // логическа€ переменна€ показывающа€ находимс€ мы на земле или нет
    private Rigidbody2D _rb; // поле Rigidbody2D дл€ физических взаимодействий
    private Input _playerInput; // система ввода

    private void Awake()
    {
        _playerInput = new Input(); // создаем экземпл€р класса Input
        _playerInput.Player.Jump.performed += context => Jump(); // подписываем метод Jump к событию нажати€ на кнопку прыжка
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // кэшируем Rigidbody2D
    }

    private void FixedUpdate()
    {
        Move(); // в FixedUpdate вызываем метод Move
        SpeedControl(); // вызываем метод SpeedControl
    }

    private void Move() // метод перемещени€ игрока
    {
        float _axis = _playerInput.Player.Move.ReadValue<float>(); // определ€ем направление движени€ 
        Vector2 _direction = new Vector2(_axis, 0f); // создаем вектор движени€ 

        _rb.AddForce(_direction * _moveSpeed, ForceMode2D.Force); // двигаемс€
    }

    private void SpeedControl() // метод контрол€ скорости
    {
        Vector2 _flatVel = new Vector2(_rb.velocity.x, 0f); // вектор текущей скорости

        if (_flatVel.magnitude > _moveSpeed) // текуща€ скорость больше скорости передвижени€
        {
            Vector2 _limitedVel = _flatVel.normalized * _moveSpeed; // вектор с нормализованной скоростью
            _rb.velocity = new Vector2(_limitedVel.x, _rb.velocity.y); // задаем движение
        }
    }

    private void Jump() // метод прыжка
    {
        if (_grounded) // находимс€ на земле
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse); // прыгаем
            _grounded = false; // больше не на земле
        }
    }

    private void OnCollisionEnter2D() => _grounded = true; // приземлились на землю

    private void OnEnable() => _playerInput.Enable(); // включаем систему ввода
  
    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}