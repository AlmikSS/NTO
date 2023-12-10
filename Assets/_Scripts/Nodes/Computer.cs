
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Computer : MonoBehaviour
{
    [SerializeField] private GameObject _quiz, _nodesUI, _back, _healthBar;//переменные для UI элементов
    [SerializeField] private PlayerController _pc;//контроллер
    [SerializeField] private Player _pl;//скрипт игрока
    [SerializeField] private PauseMenuManager _pm;//скрипт паузы
    private bool _collides = false;//состояние столкновения
    private Input _playerInput;//интпут
    private void Awake() {//создаем инпут и подписываемся на события e и esc
        _playerInput = new Input();
        _playerInput.Player.Iteract.performed += PerformIteract;
        _playerInput.UI.Escape.performed += PerformEscape;
    }
    public void OnEnable() {//активируем и деактивируем инпут
        _playerInput.Enable();
    }
    public void OnDisable() {
        _playerInput.Disable();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {//если в тригер заходит игрок, то записываем столкновение
        if(other.gameObject.tag=="Player") _collides = true;
    }
    private void OnTriggerExit2D(Collider2D other) {//когда игрок выходит
        if(other.gameObject.tag=="Player"){
            _collides = false;// !записываем столкновение
            _healthBar.SetActive(false);
            _quiz.SetActive(false);
            _nodesUI.SetActive(true);
            _back.SetActive(false);//скрываем UI
            _pc.enabled = true;
            _pl.enabled = true;
            _pm.enabled = true;//активируем скрипты
            StartCoroutine(_pl.Timer());
        }
    }
    private void PerformIteract(InputAction.CallbackContext context)
    {
        if(_collides){

            _playerInput.UI.Enable();//включаем карту инпута
            StopCoroutine(_pl.Timer());
            _pl.enabled = false;
            _pc.enabled = false;
            _pm.enabled = false;//деактивируем скрипты
            _quiz.SetActive(true);
            _nodesUI.SetActive(true);
            _healthBar.SetActive(false);
            _back.SetActive(true);//включаем UI
        }
    }
    private void PerformEscape(InputAction.CallbackContext context)
    {
        
        _collides = false;// !записываем столкновение
        _quiz.SetActive(false);
        _nodesUI.SetActive(false);
        _healthBar.SetActive(true);
        _back.SetActive(false);//скрываем UI
        //_playerInput.UI.Disable();//выключаем карту инпута
        _pc.enabled = true;
        _pl.enabled = true;
        _pm.enabled = true;//активируем скрипты
        StartCoroutine(_pl.Timer());
    }

}
