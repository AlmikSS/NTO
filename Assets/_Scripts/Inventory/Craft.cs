using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Inventory _inventory; // ссылка на инвентарь
    public List<CraftButton> CraftObjects = new List<CraftButton>(); // список всех кнопок крафта

    public void CheckRecipe(Item item) // метод крафта
    {
        for (int i = 0; i < item._itemsKey.Count; i++) // проходимся по всем нужным для крафта предметам
        {
            if (_inventory.CheckObjects(item._itemsKey[i]) < item._itemsCount[i]) return; // если предметов хватает
        }

        for (int i = 0; i < item._itemsKey.Count; i++) // проходимся по колличеству нужных предметов для крафта
        {
            _inventory.ReduseObjects(item._itemsKey[i], item._itemsCount[i]); // удаляем предметы из инвентаря
        }

        _inventory.AddItem(item); // добавляем предмет
    }

    public void Redraw()
    {
        for (int j = 0; j < CraftObjects.Count; j++) // проходимся по всем кнопкам крафта
        {
            Item item = CraftObjects[j].Item;
            for (int i = 0; i < item._itemsKey.Count; i++) // проходимся по всем нужным для крафта предметам
            {
                if (_inventory.CheckObjects(item._itemsKey[i]) < item._itemsCount[i]) // если предметов хватает
                {
                    Color color = new Color(1, 1, 1, 0.1f); // создаем цвет
                    CraftObjects[j].ChangeColor(color); // меняем цвет
                }
                else
                {
                    Color color = new Color(1, 1, 1, 1); // создаем цвет
                    CraftObjects[j].ChangeColor(color); // меняем цвет
                }
            }
        }
    }
}