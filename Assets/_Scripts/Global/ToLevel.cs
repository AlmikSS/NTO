using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevel : MonoBehaviour
{
    [SerializeField] private int SceneIndex = 0;
    [SerializeField] private Inventory _inv;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inv.Save();
            MainMenuManager.CurrentScene = SceneIndex;
            MainMenuManager.Save("Scene");
            SceneManager.LoadScene(SceneIndex);
        }
    }
}