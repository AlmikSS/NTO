using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string Name; // имя предмета
    public int Stack; // колличество предмета
    public int MaxStack; // максимальное колличество предмета в стаке
    public int ID; // ID предмета
    public ItemType ItemType; // предмет или гаджет
    public bool IsItem;

    public List<int> _itemsKey = new List<int>(); // список ID предметов нужных для крафта
    public List<int> _itemsCount = new List<int>(); // список колличества предметов нужных для крафта

    public ItemData() { }
    public ItemData(string name, Texture image, int stack, int maxStack, int iD, ItemType itemType, bool isItem, List<int> itemsKey, List<int> itemsCount)
    {
        Name = name;
        Stack = stack;
        MaxStack = maxStack;
        ID = iD;
        ItemType = itemType;
        IsItem = isItem;
        _itemsKey = itemsKey;
        _itemsCount = itemsCount;
    }
}