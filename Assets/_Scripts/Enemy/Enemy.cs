using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDamager
{
    [Header("Attack")]
    [SerializeField] private int _maxHealth; // ���� ������������� �������� ��������
    [SerializeField] private Transform _attackPoint; // ���� ����� �����
    [SerializeField] private float _attackRadius; // ������ �����
    [SerializeField] private int _damage; // ����
    [SerializeField] private LayerMask _attackMask; // ���� �������� ������� ����

    private int _health; // �������� ��������

    private void Start()
    {
        _health = _maxHealth; // ������� �������� ����� �������������
    }

    public void Attack() // ����� �����
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPoint.position.x, _attackPoint.position.y), _attackRadius, _attackMask); // �������� ���������� � ���� ������

        foreach (Collider2D col in colliders) // ���������� �� ���� �����������
        {
            col.GetComponent<Player>().TakeDamage(_damage); // ������� ����
        }
    }

    public void TakeDamage(int damage) // ����� ��������� �����
    {
        if (damage > 0) // ���� ���� ������ 0
            _health -= damage; // ������� ����
    }
}