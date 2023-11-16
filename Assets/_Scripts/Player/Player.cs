using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Close.performed += context => ShowMenuPanel();
    }

    private void ShowMenuPanel()
    {
        _menuPanel.SetActive(true);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}