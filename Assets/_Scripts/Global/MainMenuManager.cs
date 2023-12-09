using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static int CurrentScene = 1;
    private static string _path;

    public void Exit()
    {
        Save(_path);
        Application.Quit();
    }
    public void Play()
    {
        Load(_path);
        SceneManager.LoadScene(CurrentScene);
    }

    public static void Save(string path)
    {  
        _path = "Scene";
        SaveManager.SaveScene(CurrentScene, _path);
    }

    public static void Load(string path)
    {
        if (SaveManager.LoadScene(_path) > CurrentScene)
            CurrentScene = SaveManager.LoadScene(_path);
        else
            CurrentScene = 1;
    }
}