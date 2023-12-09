using System.Collections;
using UnityEngine;


public class ShootingEnemy : MonoBehaviour, IDamageable
{

    [SerializeField] private int _maxHealth, _attackDelay;
    [SerializeField] private GameObject _ball,_pivot;
    private Coroutine _attackCoroutine;
    private int _health; 
    private bool canAttack = true;
    private void Start()
    {
        _health = _maxHealth;
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(canAttack && other.gameObject.tag=="Player"){

            _attackCoroutine = StartCoroutine(AttackDelay());
        }
        
    }

    
    IEnumerator AttackDelay(){
        canAttack = false;
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("ShootingEnemy");
        GameObject _b = Instantiate(_ball, _pivot.transform.position, Quaternion.identity);
        _b.SetActive(false);
        _ball.SetActive(true);
        _ball = _b;
        yield return new WaitForSeconds(_attackDelay);
        canAttack = true;
    }
    public void TakeDamage(int damage) 
    {
        if(damage > 0) 
            _health -= damage;
        if (_health < 0)
            Die();
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }

    
}
