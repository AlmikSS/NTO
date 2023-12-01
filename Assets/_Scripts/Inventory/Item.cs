using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name; // имя предмета
    public Texture Image; // текстура предмета
    public int Stack; // колличество предмета
    public int MaxStack; // максимальное колличество предмета в стаке
    public int ID; // ID предмета
    public ItemType ItemType; // предмет или гаджет

    public List<int> _itemsKey = new List<int>(); // список ID предметов нужных для крафта
    public List<int> _itemsCount = new List<int>(); // список колличества предметов нужных для крафта
}