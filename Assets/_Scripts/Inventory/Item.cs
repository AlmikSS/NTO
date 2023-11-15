using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name;
    public Texture Image;
    public int Stack;
    public int MaxStack;
    public int ID;

    public List<int> _itemsKey = new List<int>();
    public List<int> _itemsCount = new List<int>();
}