
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Computer : MonoBehaviour
{
    [SerializeField] private GameObject _quiz, _nodesUI, _back;
    [SerializeField] private PlayerController _pc;
    [SerializeField] private Player _pl;
    [SerializeField] private PauseMenuManager _pm;
    private bool _collides = false;
    private Input _playerInput;
    private void Awake() {
        _playerInput = new Input();
        _playerInput.Player.Iteract.performed += PerformIteract;
        _playerInput.UI.Escape.performed += PerformEscape;
    }
    public void OnEnable() {
        _playerInput.Enable();
    }
    public void OnDisable() {
        _playerInput.Disable();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player") _collides = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag=="Player"){
            _collides = false;
            _quiz.SetActive(false);
            _nodesUI.SetActive(false);
            _back.SetActive(false);
            _playerInput.UI.Disable();
            _pc.enabled = true;
            _pl.enabled = true;
            _pm.enabled = true;
        }
    }
    private void PerformIteract(InputAction.CallbackContext context)
    {
        if(_collides){

            _playerInput.UI.Enable();
            _pl.enabled = false;
            _pc.enabled = false;
            _pm.enabled = false;
            _quiz.SetActive(true);
            _nodesUI.SetActive(true);
            _back.SetActive(true);
        }
    }
    private void PerformEscape(InputAction.CallbackContext context)
    {
        
        _collides = false;
        _quiz.SetActive(false);
        _nodesUI.SetActive(false);
        _back.SetActive(false);
        _playerInput.UI.Disable();
        _pc.enabled = true;
        _pl.enabled = true;
        _pm.enabled = true;
    }

}
