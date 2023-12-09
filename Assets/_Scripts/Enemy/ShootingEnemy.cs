using System.Collections;
using UnityEngine;


public class ShootingEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private int _maxHealth, _attackDelay;
    [SerializeField] private GameObject _ball,_pivot;
    private Coroutine _attackCoroutine;
    private int _health; 
    [HideInInspector] public bool canAttack = true;
    private void Start()
    {
        _health = _maxHealth;
        
    }

    public IEnumerator AttackDelay(){
        canAttack = false;
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Shoot", true);
        GameObject _b = Instantiate(_ball, _pivot.transform.position, Quaternion.identity);
        _b.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Shoot", false);
        yield return new WaitForSeconds(_attackDelay-0.6f);
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
        if (_levelManager != null)
            _levelManager.DiedEnemyCount++;
        Destroy(gameObject);
    }

    
}
