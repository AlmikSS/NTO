using UnityEngine;
using UnityEngine.SceneManagement;
public class ToLevel : MonoBehaviour
{
    [SerializeField] private int SceneIndex = 0;
    [SerializeField] private Inventory _inv;

    private void OnCollisionEnter2D(Collision2D other)
    {
        _inv.Save();
        SceneManager.LoadScene(SceneIndex);
    }
}