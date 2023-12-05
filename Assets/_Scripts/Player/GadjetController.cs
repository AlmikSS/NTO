using UnityEngine;

public class GadjetController : MonoBehaviour
{
    [SerializeField] private GadjetsInventory _inv;
    private GadjetsAbilitys _abilities;
    private PlayerController _controller;

    private Input _playerInput;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _abilities = GetComponent<GadjetsAbilitys>();

        _playerInput = new Input();
        _playerInput.Player.Gadjet1.performed += context => AbilityOnGadjet(_inv.Items[0]);
        _playerInput.Player.Gadjet2.performed += context => AbilityOnGadjet(_inv.Items[1]);
        _playerInput.Player.Gadjet3.performed += context => AbilityOnGadjet(_inv.Items[2]);
        _playerInput.Player.Gadjet4.performed += context => AbilityOnGadjet(_inv.Items[3]);
    }

    private void AbilityOnGadjet(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.DoubleJumpGadjet:
                if (_abilities.ReadyToDoubleJump)
                    StartCoroutine(_abilities.DoubleJump(_controller));
                break;
            case ItemType.ShieldGadjet:
                if (_abilities.ReadyToInstShield)
                    StartCoroutine(_abilities.ShieldGadjet(_controller.transform));
                break;
        }
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}