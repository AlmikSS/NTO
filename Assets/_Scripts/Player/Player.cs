using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _nodsPanel;

    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.NodsPanelOpen.performed += context => ShowNodsPanel();
        _playerInput.Player.Close.performed += context => CloseNodsPanel();
    }

    private void ShowNodsPanel()
    {
        _nodsPanel.SetActive(true);
    }

     private void CloseNodsPanel()
    {
        _nodsPanel.SetActive(false);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}