using UnityEngine;

public class ShootingTrigger : MonoBehaviour
{
    private ShootingEnemy _se;
    private void Awake() {
        _se = gameObject.GetComponentInParent<ShootingEnemy>();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(_se.canAttack && other.gameObject.tag=="Player"){

            StartCoroutine(_se.AttackDelay());
        }
        
    }
}
