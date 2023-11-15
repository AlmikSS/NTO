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

    private void Start()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            GameObject newSlot = Instantiate(_button);
            newSlot.transform.SetParent(_inventoryPanel, false);
            newSlot.GetComponent<InventoryButton>().MyInv = this;
            newSlot.GetComponent<InventoryButton>().MyID = i;
        }

        for (int i = 0; i < _width * _height; i++)
        {
            GameObject newItem = new GameObject("Item", typeof(Item));

            newItem.GetComponent<Item>().Name = Items[i].Name;
            newItem.GetComponent<Item>().Stack = Items[i].Stack;
            newItem.GetComponent<Item>().MaxStack = Items[i].MaxStack;
            newItem.GetComponent<Item>().ID = Items[i].ID;
            newItem.GetComponent<Item>().Image = Items[i].Image;

            Items[i] = newItem.GetComponent<Item>();
        }
        Redraw();
    }

    private void Update()
    {
        _mouseItemImage.transform.position = Input.mousePosition - new Vector3(0, 0, 0);
    }

    public void SelectSlot(int ID)
    {
        if (Items[ID].ID == _mouseItem.ID)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
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

    public void Redraw()
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
}