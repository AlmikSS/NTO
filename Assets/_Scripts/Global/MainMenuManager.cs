using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static int CurrentScene = 1;

    public void Exit()
    {
        Save("Scene");
        Application.Quit();
    }
    public void Play()
    {
        Load("Scene");
        SceneManager.LoadScene(CurrentScene);
    }

    public static void Save(string path)
    {  
        SaveManager.SaveScene(CurrentScene, path);
    }

    public static void Load(string path)
    {
        if (SaveManager.LoadScene("Scene") > CurrentScene)
            CurrentScene = SaveManager.LoadScene("Scene");
        else
            CurrentScene = 1;
    }
}