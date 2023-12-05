using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private int CurrentScene = 1;

    private void Start()
    {
        int scene = PlayerPrefs.GetInt("Scene");
        if (scene != CurrentScene) 
            CurrentScene = scene;
    }

    public void Exit() => Application.Quit();

    public void Play()
    {
        SceneManager.LoadScene(CurrentScene);
    }
}