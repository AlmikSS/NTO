using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Inventory _inventory; // ссылка на инвентарь

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
}