using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDamager
{
    [Header("Attack")]
    [SerializeField] private int _maxHealth; // поле максимального значения здоровья
    [SerializeField] private Transform _attackPoint; // поле точки атаки
    [SerializeField] private float _attackRadius; // радиус атаки
    [SerializeField] private int _damage; // урон
    [SerializeField] private LayerMask _attackMask; // слой которому наносим урон

    private int _health; // текующее здоровье

    private void Start()
    {
        _health = _maxHealth; // текущее здоровье равно максимальному
    }

    public void Attack() // метод атаки
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPoint.position.x, _attackPoint.position.y), _attackRadius, _attackMask); // собираем коллайдеры в зоне аттаки

        foreach (Collider2D col in colliders) // проходимся по всем коллайдерам
        {
            col.GetComponent<Player>().TakeDamage(_damage); // наносим урон
        }
    }

    public void TakeDamage(int damage) // метод получения урона
    {
        if (damage > 0) // если урон больше 0
            _health -= damage; // наносим урон
    }
}