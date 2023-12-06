
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillingFlor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
