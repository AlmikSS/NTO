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

    [SerializeField] private GadjetsVisualization _visualization;
    [SerializeField] private Item _nullItem; // пустой предмет
    public List<Item> Items = new List<Item>(4); // список всех предметов в инвентаре

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
            GameObject _newItem = new GameObject("Gadjet", typeof(Item)); // создаем предмет

            //задаем характеристики предмета
            _newItem.GetComponent<Item>().Name = Items[i].Name;
            _newItem.GetComponent<Item>().ID = Items[i].ID;
            _newItem.GetComponent<Item>().Stack = Items[i].Stack;
            _newItem.GetComponent<Item>().MaxStack = Items[i].MaxStack;
            _newItem.GetComponent<Item>().Image = Items[i].Image;
            _newItem.GetComponent<Item>().ItemType = Items[i].ItemType;
            //добавляем предмет в список
            Items[i] = _newItem.GetComponent<Item>();
        }
        
        Redraw(); // перерисовываем весь инвентарь
    }

    public void SelectSlot(int ID) // метод выбора слота
    {
        if (_inv.MouseItem.ItemType != ItemType.Item)
        {
            Item tempItem = Items[ID]; // временное поле для хранения предмета в слоте
            Items[ID] = _inv.MouseItem; // в слоте теперь предмет который был в руке
            _inv.MouseItem = tempItem; // в руке теперь предмет который был в слоте
        }

        Redraw(); // перерисовываем инвентарь
    }

    private void Redraw() // метод перерисовки инвентаря
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по всем слотам
        {
            _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().texture = Items[i].Image; // меняем текстуру слота на текстуру предмета в этом слоте

            if (Items[i].ItemType != ItemType.Null)
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            else if (Items[i].ItemType == ItemType.Null)
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }

        _inv.Redraw();
        _visualization.Redraw();
    }

    private void OnEnable() => _playerInput.Enable(); // включем систему ввода

    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}