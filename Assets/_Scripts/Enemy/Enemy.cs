using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class Enemy : MonoBehaviour, IDamageable, IDamager
{
    [Header("Attack")]
    [SerializeField] private int _maxHealth; // поле максимального значения здоровья
    [SerializeField] private Transform _attackPoint; // поле точки атаки
    [SerializeField] private float _attackRadius; // радиус атаки
    [SerializeField] private int _damage; // урон
    [SerializeField] private LayerMask _attackMask; // слой которому наносим урон
    [SerializeField] private float _attackDistance; // дистанция на которой враг начинает аттаковать
    [SerializeField] private bool _canShoot = false; // переменная может ли враг стрелять

    private AIDestinationSetter _destinationSetter; // скрипт нахождения пути до игрока
    private Vector3 _scale; // размер врага
    private int _health; // текующее здоровье

    private void Start()
    {
        _health = _maxHealth; // текущее здоровье равно максимальному
        _scale = transform.localScale; // кэшируем размер
        _destinationSetter = GetComponent<AIDestinationSetter>(); // кэшируем AIDestinationSetter
    }

    public void Attack() // метод атаки
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPoint.position.x, _attackPoint.position.y), _attackRadius, _attackMask); // собираем коллайдеры в зоне аттаки

        foreach (Collider2D col in colliders) // проходимся по всем коллайдерам
        {
            col.GetComponent<Player>().TakeDamage(_damage); // наносим урон
        }
    }

    private void Update()
    {
        if ((transform.position.x - _destinationSetter.CurrentTarget.position.x) < 0) // двигаемся вправо
            transform.localScale = _scale; // смотрим вправо
        else if ((transform.position.x - _destinationSetter.CurrentTarget.position.x) > 0) // двигаемся влево
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z); // смотрим влево
    }

    public void TakeDamage(int damage) // метод получения урона
    {
        if (damage > 0) // если урон больше 0
            _health -= damage; // наносим урон
        if (_health < 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}