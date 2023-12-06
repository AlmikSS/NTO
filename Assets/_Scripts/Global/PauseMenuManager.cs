using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanal;
    [SerializeField] private Player _player;

    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Close.performed += context => Stop();
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Stop()
    {
        _pausePanal.SetActive(!_pausePanal.activeSelf);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else if (Time.timeScale == 1)
            Time.timeScale = 0;
    }

    public void PlayButton()
    {
        _pausePanal.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
        SaveManager.Save(_player);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}