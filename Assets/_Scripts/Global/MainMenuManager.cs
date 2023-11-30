using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private int _levelSceneNuber;

    public void Exit() => Application.Quit();

    public void Play()
    {
        SceneManager.LoadScene(1);
        StartCoroutine(LoadLevel.Load(_levelSceneNuber));
    }
}