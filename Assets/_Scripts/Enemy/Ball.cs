using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int _damage = 15;
    [SerializeField] private float speed = 0.1f;
    private void OnEnable() {
        StartCoroutine(DeactiveDelay(2));
    }
    private void Update() {
        transform.position = new Vector2(transform.position.x+speed, transform.position.y);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(_damage); }
        GetComponent<Animator>().SetTrigger("Caboom");
        StartCoroutine(DeactiveDelay(0.3f));
    }
    IEnumerator DeactiveDelay(float i){
        yield return new WaitForSeconds(i);
        GetComponent<Animator>().SetTrigger("Caboom");
        Destroy(gameObject);
    }

}
