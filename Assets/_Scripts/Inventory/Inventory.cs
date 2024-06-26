﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _width, _height; // ширина и высота инвентаря
    [SerializeField] public Transform _inventoryPanel; // ссылка на объект панели инвентаря
    [SerializeField] private GameObject _button; // сылка на префаб слота/кнопки инвентаря
    [SerializeField] private GadjetsInventory _gadjetInventory;

    [SerializeField] private List<Item> _items = new List<Item>(10); // список всех предметов в инвентаре
    [SerializeField] private Item _nullItem; // пустой предмет
    [SerializeField] public RawImage MouseItemImage; // текстура предмета в руке
    [SerializeField] private Craft _craftMenu;
    [SerializeField] private Images _images;
    private List<InventoryButton> _slots = new List<InventoryButton>();
    private List<ItemData> _itemsData = new List<ItemData>();
    private List<GameObject> _itemObjects = new List<GameObject>();
    private string _path = "inventory";
    public Item MouseItem; // предмет в руке
    public Transform InfoSpawn;

    private Input _playerInput; // система ввода игрока

    public void Awake()
    {
        _path = "inventory";
        _playerInput = new Input(); // создаем экземпляр класса Input
    }

    public void Save()
    {
        SaveManager.Save(_itemsData, _path);
        _gadjetInventory.Save();
    }

    private void Load()
    {
        _itemsData = SaveManager.Load(_path);

        for (int i = 0; i < _itemObjects.Count; i++)
        {
            Destroy(_itemObjects[i]);
        }

        _itemObjects.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            GameObject _newItem = new GameObject("Item", typeof(Item)); // создаем предмет

            //задаем характеристики предмета
            _newItem.GetComponent<Item>().Name = _itemsData[i].Name;
            _newItem.GetComponent<Item>().ID = _itemsData[i].ID;
            _newItem.GetComponent<Item>().Stack = _itemsData[i].Stack;
            _newItem.GetComponent<Item>().MaxStack = _itemsData[i].MaxStack;
            _newItem.GetComponent<Item>().Image = SetImage(_itemsData[i].ItemType);
            _newItem.GetComponent<Item>().ItemType = _itemsData[i].ItemType;
            _newItem.GetComponent<Item>().Data = _itemsData[i];
            _newItem.GetComponent<Item>().IsItem = _itemsData[i].IsItem;
            _newItem.GetComponent<Item>().Inizialize();
            //добавляем предмет в список
            _items[i] = _newItem.GetComponent<Item>();
            _itemObjects.Add(_newItem);
        }
        _gadjetInventory.Load();
        Redraw();
    }

    private Texture SetImage(ItemType type)
    {
        if (type == ItemType.Null)
            return _images.NullItemTexture;
        else if (type == ItemType.ShieldGadjet)
            return _images.ShieldGadjetTexture;
        else if (type == ItemType.DoubleJumpGadjet)
            return _images.DoubleJumpTexture;
        else if (type == ItemType.Fragment)
            return _images.FragmentTexture;
        return null;
    }

    private void Start()
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по колличеству слотов
        {
            GameObject _newSlot = Instantiate(_button); // создаем слот
            _newSlot.transform.SetParent(_inventoryPanel, false); // делаем созданный слот дочерним объектом от _inventoryPanel
            _newSlot.GetComponent<InventoryButton>().MyInv = this; // добавим кнопке ссылку на инвентарь
            _newSlot.GetComponent<InventoryButton>().MyID = i; // добавим слоту ID
            _slots.Add(_newSlot.GetComponent<InventoryButton>());
        }

        for (int i = 0; i < _height * _width; i++) // проходимся по всем слотам
        {
            GameObject _newItem = new GameObject("Item", typeof(Item)); // создаем предмет

            //задаем характеристики предмета
            _newItem.GetComponent<Item>().Name = _items[i].Name;
            _newItem.GetComponent<Item>().ID = _items[i].ID;
            _newItem.GetComponent<Item>().Stack = _items[i].Stack;
            _newItem.GetComponent<Item>().MaxStack = _items[i].MaxStack;
            _newItem.GetComponent<Item>().Image = _items[i].Image;
            _newItem.GetComponent<Item>().ItemType = _items[i].ItemType;
            _newItem.GetComponent<Item>().Data = _items[i].Data;
            _newItem.GetComponent<Item>().IsItem = _items[i].IsItem;
            _newItem.GetComponent<Item>().Inizialize();
            //добавляем предмет в список
            _items[i] = _newItem.GetComponent<Item>();
            _itemObjects.Add(_newItem);
            _itemsData.Add(_items[i].Data);
        }

        if (SaveManager.Load(_path) != null)
            Load();

        Redraw(); // перерисовываем весь инвентарь
        _craftMenu.Redraw(); // перерисовывем меню крафта
    }

    private void Update()
    {
        MouseItemImage.transform.position = _playerInput.Player.MousePosition.ReadValue<Vector2>(); // перемещение объекта в руку
    }

    private void UpdateData()
    {
        for (int i = 0; i < _height * _width; i++)
        {
            _itemsData[i] = _items[i].Data;
        }
    }

    public void SelectSlot(int ID) // метод выбора слота
    {
        if (_items[ID].ID == MouseItem.ID) // если предмет в руке и предмет в слоте одинаковые
        {
            if (!_playerInput.Player.TakeAllStack.IsPressed()) // если нажата кнопка TakeAllStack
            {
                if (MouseItem.Stack > _items[ID].MaxStack - _items[ID].Stack) // если превысили размер стака
                {
                    MouseItem.Stack -= _items[ID].MaxStack - _items[ID].Stack; // ставим в инвентарь часть предметов
                    _items[ID].Stack = _items[ID].MaxStack; // в слоте теперь полный стак
                }
                else // если не превысили стак
                {
                    _items[ID].Stack += MouseItem.Stack; // добавляем предметы в слот
                    // в руке теперь пустота
                    MouseItem.Name = _nullItem.Name;
                    MouseItem.Image = _nullItem.Image;
                    MouseItem.Stack = 0;
                    MouseItem.MaxStack = 0;
                    MouseItem.ID = 0;
                }
            }
            else // ставим вещи по одному
            {
                if (MouseItem.Stack > 1 && _items[ID].Stack < _items[ID].MaxStack) // если предметов в слоте меньше стака
                {
                    _items[ID].Stack++; // увеличиваем колличество предметов в слоте
                    MouseItem.Stack--; // уменьшаем колличество предметов в руке
                }
            }
        }

        else // меняем предмет в руке 
        {
            Item tempItem = _items[ID]; // временное поле для хранения предмета в слоте
            _items[ID] = MouseItem; // в слоте теперь предмет который был в руке
            MouseItem = tempItem; // в руке теперь предмет который был в слоте
        }

        Redraw(); // перерисовываем инвентарь
    }

    public void Redraw() // метод перерисовки инвентаря
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по всем слотам
        {
            _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().texture = _items[i].Image; // меняем текстуру слота на текстуру предмета в этом слоте

            if (_items[i].ItemType != ItemType.Null)
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            else if (_items[i].ItemType == ItemType.Null)
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

            if (_items[i].ID == 0 || _items[i].Stack == 0) // если предмета нет
            {
                _inventoryPanel.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = ""; // меняем текст на пустоту
            }
            else // предмет есть
            {
                _inventoryPanel.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = _items[i].Stack.ToString(); // меняем текст на колличество предмета
            }
        }

        if (MouseItem.Image == null) // в руке ничего нет
        {
            MouseItemImage.GetComponent<RawImage>().color = new Color(0, 0, 0, 0); // делаем текстуру прозрачной
            MouseItemImage.transform.GetChild(0).GetComponent<TMP_Text>().text = ""; // меняем текст на пустоту
        }
        else
        {
            MouseItemImage.GetComponent<RawImage>().color = new Color(1, 1, 1, 1); // делаем текстуру видимой
            MouseItemImage.GetComponent<RawImage>().texture = MouseItem.Image; // меняем текстуру в руке
            MouseItemImage.transform.GetChild(0).GetComponent<TMP_Text>().text = MouseItem.Stack.ToString(); // меняем текст в руке
        }

        UpdateSlots();
        UpdateData();
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _slots[i].ItemInSlot = _items[i];
        }
    }

    public int CheckObjects(int id) // метод проверки есть ли предмет в инвентаре
    {
        int _objectsCount = 0; // общее колличество предметов

        for (int i = 0; i < _width * _height; i++) // проходимся по всем слотам
        {
            if (_items[i].ID == id) // если нашли предмет
            {
                _objectsCount += _items[i].Stack; // увеличиваем колличество найденных предметов
            }
        }

        return _objectsCount; // возвращаем колличество найденных предметов
    }

    public void ReduseObjects(int id, int count) // метод удаления объектов
    {
        for (int i = 0; i < _width * _height; i++) // проходимся по всем слотам
        {
            if (_items[i].ID == id) // нашли нужный предмет
            {
                if (_items[i].Stack >= count) // если предмета хватате
                {
                    if (_items[i].Stack == count) // если предмета хватает ровно
                    {
                        _items[i] = _nullItem; // в слоте теперь пусто
                    }
                    _items[i].Stack -= count; // уменьшаем колличество предмета
                    break; // выходим из цикла
                }
                else
                {
                    count -= _items[i].Stack; // уменьшаем коллиество нужного предмета
                    _items[i] = _nullItem; // в слоте теперь пустота
                }
            }
        }

        Redraw(); // перерисовываем инвентарь
        _craftMenu.Redraw(); // перерисовывем меню крафта
    }

    public void AddItem(Item newItem) // метод добавления предметов в инвентарь
    {
        for (int i = 0; i < _width * _height; i++)  // проходимся по всем слотам
        {
            if (CheckObjects(newItem.ID) > 0 && _items[i].Stack < _items[i].MaxStack) // если такой предмет уже есть
            {
                if (_items[i].ID == newItem.ID) // ID совпадает
                {
                    _items[i].Stack++; // увеличиваем колличество предмета
                    break; // выходим из цикла
                }
            }
            else if (_items[i].ID == 0) // ищем пустой слот
            {
                GameObject newItemInv = new GameObject("Item", typeof(Item)); // создаем новый предмет
                //задаем характеристики
                newItemInv.GetComponent<Item>().Name = newItem.Name;
                newItemInv.GetComponent<Item>().ID = newItem.ID;
                newItemInv.GetComponent<Item>().Stack = newItem.Stack;
                newItemInv.GetComponent<Item>().MaxStack = newItem.MaxStack;
                newItemInv.GetComponent<Item>().Image = newItem.Image;
                newItemInv.GetComponent<Item>().ItemType = newItem.ItemType;
                //newItemInv.GetComponent<Item>().Data = newItem.Data;
                newItemInv.GetComponent<Item>().IsItem = newItem.IsItem;
                newItemInv.GetComponent<Item>().Inizialize();
                //добавляем предмет в список
                _items[i] = newItemInv.GetComponent<Item>();
                _itemObjects.Add(newItemInv);
                break; // выходим из цикла
            }
        }

        Redraw(); // перерисовываем инвентарь
        UpdateData();
        _craftMenu.Redraw(); // перерисовывем меню крафта
    }

    private void OnEnable() => _playerInput.Enable(); // включем систему ввода

    private void OnDisable() => _playerInput.Disable(); // выключаем систему ввода
}