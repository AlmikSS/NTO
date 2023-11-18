using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    private int _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
            _health -= damage;
    }
}