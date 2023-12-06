using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private int CurrentScene = 1;

    public void Exit() => Application.Quit();

    public void Play()
    {
        SceneManager.LoadScene(CurrentScene);
    }
}