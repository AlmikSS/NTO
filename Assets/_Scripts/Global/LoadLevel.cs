using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;
    [SerializeField] private TMP_Text _loadingText;
    private static LoadLevel _instance;

    private void Start()
    {
        _instance = this;
    }

    public static IEnumerator Load(int sceneNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        operation.allowSceneActivation = false;

        float startTime = Time.time;
        _instance._loadingImage.fillAmount = operation.progress;
        _instance._loadingText.text = Mathf.RoundToInt(operation.progress) * 100 + "%";

        if (operation.isDone)
        {
            if (Time.time - startTime < 2)
            {
                yield return new WaitForSeconds(2);
                operation.allowSceneActivation = true;
            }
            else
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}