using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _damage;

    private void Start()
    {
        Invoke("Die", _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.TryGetComponent(out ShootingEnemy enemy1))
        {
            enemy1.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}