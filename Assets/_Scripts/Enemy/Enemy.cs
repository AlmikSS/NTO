using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class Enemy : MonoBehaviour, IDamageable, IDamager
{
    [Header("Attack")]
    [SerializeField] private int _maxHealth; // ���� ������������� �������� ��������
    [SerializeField] private Transform _attackPoint; // ���� ����� �����
    [SerializeField] private float _attackRadius; // ������ �����
    [SerializeField] private int _damage; // ����
    [SerializeField] private LayerMask _attackMask; // ���� �������� ������� ����
    [SerializeField] private float _attackDistance; // ��������� �� ������� ���� �������� ����������
    [SerializeField] private bool _canShoot = false; // ���������� ����� �� ���� ��������

    private AIDestinationSetter _destinationSetter; // ������ ���������� ���� �� ������
    private Vector3 _scale; // ������ �����
    private int _health; // �������� ��������

    private void Start()
    {
        _health = _maxHealth; // ������� �������� ����� �������������
        _scale = transform.localScale; // �������� ������
        _destinationSetter = GetComponent<AIDestinationSetter>(); // �������� AIDestinationSetter
    }

    public void Attack() // ����� �����
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPoint.position.x, _attackPoint.position.y), _attackRadius, _attackMask); // �������� ���������� � ���� ������

        foreach(Collider2D col in colliders) // ���������� �� ���� �����������
        {
            col.GetComponent<Player>().TakeDamage(_damage); // ������� ����
        }
    }

    private void Update()
    {
        if((transform.position.x - _destinationSetter.CurrentTarget.position.x) < 0) // ��������� ������
            transform.localScale = _scale; // ������� ������
        else if((transform.position.x - _destinationSetter.CurrentTarget.position.x) > 0) // ��������� �����
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z); // ������� �����
    }

    public void TakeDamage(int damage) // ����� ��������� �����
    {
        if(damage > 0) // ���� ���� ������ 0
            _health -= damage; // ������� ����
        if (_health < 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
