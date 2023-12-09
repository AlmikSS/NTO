using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static int CurrentScene = 1;
    private string _path;

    public void Exit()
    {
        _path = CurrentScene + "/Scene.xml";
        SaveManager.SaveScene(CurrentScene, _path);
        Application.Quit();
    }
    public void Play()
    {
        if (SaveManager.LoadScene(_path) != CurrentScene)
            CurrentScene = SaveManager.LoadScene(_path);

        SceneManager.LoadScene(CurrentScene);
    }
}