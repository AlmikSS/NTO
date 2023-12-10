using System.Collections;
using System.Linq;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField] private int _maxHealth; // ���� ������������� �������� ��������
    [SerializeField] private Transform _attackPoint; // ���� ����� �����
    [SerializeField] private float _attackRadius; // ������ �����
    [SerializeField] private int _damage;// ����
    [SerializeField] private int _attackDelay = 2; 
    [SerializeField] private LayerMask _attackMask; // ���� �������� ������� ����
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private LevelManager _levelManager;
    private bool canAttack = true;
    private AIDestinationSetter _destinationSetter; // ������ ���������� ���� �� ������
    private Vector3 _scale; // ������ �����
    private int _health; // �������� ��������

    private void Start()
    {
        _health = _maxHealth; // ������� �������� ����� �������������
        _scale = transform.localScale; // �������� ������
        _destinationSetter = GetComponent<AIDestinationSetter>(); // �������� AIDestinationSetter
    }


    IEnumerator AttackDelay(Collider2D[] colliders){
        gameObject.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack", true);
        canAttack = false;
        foreach(Collider2D col in colliders) // ���������� �� ���� �����������
        {
            col.GetComponent<Player>().TakeDamage(_damage); // ������� ����
        }
        yield return new WaitForSeconds(0.45f);
        gameObject.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack", false);
        yield return new WaitForSeconds(_attackDelay-0.45f);
        canAttack = true;
    }
    private void Update()
    {
        
        if((transform.position.x - _destinationSetter.CurrentTarget.position.x) < 0) // ��������� ������
            transform.localScale = _scale; // ������� ������
        else if((transform.position.x - _destinationSetter.CurrentTarget.position.x) > 0) // ��������� �����
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z); // ������� �����
        if(_destinationSetter.CurrentTarget.gameObject.tag=="Player"  && canAttack){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(_attackPoint.position.x, _attackPoint.position.y), _attackRadius, _attackMask); // �������� ���������� � ���� ������

            if(colliders.Length>0) StartCoroutine(AttackDelay(colliders));
        }
    }

    public void TakeDamage(int damage) // ����� ��������� �����
    {
        if(damage > 0) // ���� ���� ������ 0
        {
            _health -= damage;
            StartCoroutine(ChangeColor());
        }
        if (_health < 0)
            Die();
    }

    private IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        _spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        if (_levelManager != null)
            _levelManager.DiedEnemyCount++;
        Destroy(gameObject);
    }
}
