using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : MonoBehaviour
{
    public string Name; // ��� ��������
    public Texture Image; // �������� ��������
    public int Stack; // ����������� ��������
    public int MaxStack; // ������������ ����������� �������� � �����
    public int ID; // ID ��������
    public ItemType ItemType; // ������� ��� ������
    public bool IsItem;

    public List<int> _itemsKey = new List<int>(); // ������ ID ��������� ������ ��� ������
    public List<int> _itemsCount = new List<int>(); // ������ ����������� ��������� ������ ��� ������

    public ItemData Data;

    public void Inizialize()
    {
        Data = new ItemData(Name, Image, Stack, MaxStack, ID, ItemType, IsItem, _itemsKey, _itemsCount);
    }
}