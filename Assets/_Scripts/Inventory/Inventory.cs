using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Transform _inventoryPanel;
    [SerializeField] private GameObject _button;

    public List<Item> Items = new List<Item>();
    [SerializeField] private Item _mouseItem, _nullItem;
    [SerializeField] private RawImage _mouseItemImage;

    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
    }

    private void Start()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            GameObject _newSlot = Instantiate(_button);
            _newSlot.transform.SetParent(_inventoryPanel, false);
            _newSlot.GetComponent<InventoryButton>().MyInv = this;
            _newSlot.GetComponent<InventoryButton>().MyID = i;
        }

        for (int i = 0; i < _height * _width; i++)
        {
            GameObject _newItem = new GameObject("Item", typeof(Item));

            _newItem.GetComponent<Item>().Name = Items[i].Name;
            _newItem.GetComponent<Item>().ID = Items[i].ID;
            _newItem.GetComponent<Item>().Stack = Items[i].Stack;
            _newItem.GetComponent<Item>().MaxStack = Items[i].MaxStack;
            _newItem.GetComponent<Item>().Image = Items[i].Image;

            Items[i] = _newItem.GetComponent<Item>();
        }
        Redraw();
    }

    private void Update()
    {
        _mouseItemImage.transform.position = _playerInput.Player.MousePosition.ReadValue<Vector2>();
    }

    public void SelectSlot(int ID)
    {
        if (Items[ID].ID == _mouseItem.ID)
        {
            if (!_playerInput.Player.TakeAllStack.IsPressed())
            {
                if (_mouseItem.Stack > Items[ID].MaxStack - Items[ID].Stack)
                {
                    _mouseItem.Stack -= Items[ID].MaxStack - Items[ID].Stack;
                    Items[ID].Stack = Items[ID].MaxStack;
                }
                else
                {
                    Items[ID].Stack += _mouseItem.Stack;
                    _mouseItem.name = _nullItem.Name;
                    _mouseItem.Image = _nullItem.Image;
                    _mouseItem.Stack = 0;
                    _mouseItem.MaxStack = 0;
                    _mouseItem.ID = 0;
                }
            }
            else
            {
                if (_mouseItem.Stack > 1 &&
                    Items[ID].Stack < Items[ID].MaxStack)
                {
                    Items[ID].Stack++;
                    _mouseItem.Stack--;
                }
            }
        }

        else
        {
            Item tempItem = Items[ID];
            Items[ID] = _mouseItem;
            _mouseItem = tempItem;
        }
        Redraw();
    }

    private void Redraw()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            _inventoryPanel.GetChild(i).GetChild(0).GetComponent<RawImage>().texture = Items[i].Image;
            
            if (Items[i].ID == 0 || Items[i].Stack == 0)
            {
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            else
            {
                _inventoryPanel.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = Items[i].Stack.ToString();
            }
        }

        if (_mouseItem.Image == null)
        {
            _mouseItemImage.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            _mouseItemImage.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        }
        else
        {
            _mouseItemImage.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            _mouseItemImage.GetComponent<RawImage>().texture = _mouseItem.Image;
            _mouseItemImage.transform.GetChild(0).GetComponent<TMP_Text>().text = _mouseItem.Stack.ToString();
        }
    }

    public int CheckObjects(int id)
    {
        int _objectsCount = 0;

        for (int i = 0; i < _width * _height; i++)
        {
            if (Items[i].ID == id)
            {
                _objectsCount += Items[i].Stack;
            }
        }

        return _objectsCount;
    }

    public void ReduseObjects(int id, int count)
    {
        for (int i = 0; i < _width * _height; i++)
        {
            if (Items[i].ID == id)
            {
                if (Items[i].Stack >= count)
                {
                    if (Items[i].Stack == count)
                    {
                        Items[i] = _nullItem;
                    }
                    Items[i].Stack -= count;
                    break;
                }
                else
                {
                    count -= Items[i].Stack;
                    Items[i] = _nullItem;
                }
            }
        }
        Redraw();
    }

    public void AddItem(Item newItem)
    {
        for (int i = 0; i < _width * _height; i++)
        {
            if (CheckObjects(newItem.ID) > 0)
            {
                if (Items[i].ID == newItem.ID)
                {
                    Items[i].Stack++;
                    break;
                }
            }
            else if (Items[i].ID == 0)
            {
                GameObject newItemInv = new GameObject("Item", typeof(Item));

                newItemInv.GetComponent<Item>().name = newItem.Name;
                newItemInv.GetComponent<Item>().ID = newItem.ID;
                newItemInv.GetComponent<Item>().Image = newItem.Image;
                newItemInv.GetComponent<Item>().Stack = 1;
                newItemInv.GetComponent<Item>().MaxStack = newItem.MaxStack;

                Items[i] = newItemInv.GetComponent<Item>();
                break;
            }
        }
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}