using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _attackRadius;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;
    private int _health;

    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Attack.performed += context => Attack();
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPosition.position.x, _attackPosition.position.y), _attackRadius, _attackMask);

        foreach (Collider2D enemy in colliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
            _health -= damage;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}