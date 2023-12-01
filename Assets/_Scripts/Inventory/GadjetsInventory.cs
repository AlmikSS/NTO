using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadjetsInventory : MonoBehaviour
{
    [SerializeField] private int _width, _height; // ширина и высота инвентаря
    [SerializeField] private Transform _inventoryPanel; // ссылка на объект панели инвентаря
    [SerializeField] private GameObject _button; // сылка на префаб слота/кнопки инвентаря
    [SerializeField] private Inventory _inv;

    [SerializeField] private List<Item> _items = new List<Item>(4); // список всех предметов в инвентаре
    [SerializeField] private Item _nullItem; // пустой предмет

    public List<Gadjet> Gadjets = new List<Gadjet>();
    private Input _playerInput; // система ввода игрока

    private void Awake()
    {
        _playerInput = new Input(); // создаем экземпляр класса Input
    }

    private void Start()
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по колличеству слотов
        {
            GameObject _newSlot = Instantiate(_button); // создаем слот
            _newSlot.transform.SetParent(_inventoryPanel, false); // делаем созданный слот дочерним объектом от _inventoryPanel
            _newSlot.GetComponent<GadjetInventoryButton>().MyInv = this; // добавим кнопке ссылку на инвентарь
            _newSlot.GetComponent<GadjetInventoryButton>().MyID = i; // добавим слоту ID
        }

        for (int i = 0; i < _height * _width; i++) // проходимся по всем слотам
        {
            GameObject _newGadjet = new GameObject("Gadjet", typeof(Gadjet)); // создаем предмет
            //добавляем предмет в список
            Gadjets[i] = _newGadjet.GetComponent<Gadjet>();
        }
        
        Redraw(); // перерисовываем весь инвентарь
    }

    public void SelectSlot(int ID) // метод выбора слота
    {
        if (_inv.MouseItem.ItemType == ItemType.Gadjet || _inv.MouseItem.ItemType == ItemType.Null)
        {
            Gadjet tempItem = Gadjets[ID].GetComponent<Gadjet>(); // временное поле для хранения предмета в слоте
            Gadjets[ID] = _inv.MouseItem.GetComponent<Gadjet>(); // в слоте теперь предмет который был в руке
            _inv.MouseItem = tempItem.GetComponent<Item>(); // в руке теперь предмет который был в слотеSs
        }

        Redraw(); // перерисовываем инвентарь
    }

    private void Redraw() // метод перерисовки инвентаря
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по всем слотам
        {
            _inventoryPanel.GetChild(i).GetChild(0).GetComponent<RawImage>().texture = _items[i].Image; // меняем текстуру слота на текстуру предмета в этом слоте

            if (_items[i].ID == 0 || _items[i].Stack == 0) // если предмета нет
            {
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = ""; // меняем текст на пустоту
            }
            else // предмет есть
            {
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = _items[i].Stack.ToString(); // меняем текст на колличество предмета
            }
        }

        _inv.Redraw();
    }

    private void OnEnable() => _playerInput.Enable(); // включем систему ввода

    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}