using UnityEngine;
using UnityEngine.SceneManagement;
public class ToLevel : MonoBehaviour
{
    [SerializeField] private int SceneIndex = 0;
    private void OnCollisionEnter2D(Collision2D other) {
            SceneManager.LoadScene(SceneIndex);
        
    }
}
