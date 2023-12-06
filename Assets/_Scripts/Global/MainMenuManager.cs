using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private int CurrentScene = 1;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Scene"))
            CurrentScene = PlayerPrefs.GetInt("Scene");
    }

    public void Exit() => Application.Quit();

    public void Play()
    {
        SceneManager.LoadScene(CurrentScene);
    }
}