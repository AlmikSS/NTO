
using UnityEngine;
using UnityEngine.InputSystem;

public class Computer : MonoBehaviour
{
    [SerializeField] private GameObject[] ToHide;//_quiz, _nodesUI, _back
    [SerializeField] private GameObject[] ToActivate;// _healthBar, _gadjetPanel
    [SerializeField] private PlayerController _pc;//контроллер
    [SerializeField] private Player _pl;//скрипт игрока
    [SerializeField] private PauseMenuManager _pm;//скрипт паузы
    private bool _collides = false;//состояние столкновения
    private Input _playerInput;//интпут
    private float _stoppedHealth;
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
    private void PerformIteract(InputAction.CallbackContext context)
    {
        if(_collides){

            _playerInput.UI.Enable();//включаем карту инпута
            _pl.enabled = false;
            _pc.enabled = false;
            _pm.enabled = false;//деактивируем скрипты
            foreach(GameObject x in ToHide) x.SetActive(true);//открываем UI нодов
            foreach(GameObject x in ToActivate) x.SetActive(false);//скрываем UI игры
            Time.timeScale = 0;
        }
    }
    private void PerformEscape(InputAction.CallbackContext context)
    {
        
        _collides = false;// !записываем столкновение
        foreach(GameObject x in ToActivate) x.SetActive(true);//открываем UI игры
        foreach(GameObject x in ToHide) x.SetActive(false);//скрываем UI нодов
        _pc.enabled = true;
        _pl.enabled = true;
        _pm.enabled = true;//активируем скрипты
        Time.timeScale = 1;
        
    }

}
