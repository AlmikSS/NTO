using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    public void CheckRecipe(Item item)
    {
        for (int i = 0; i < item._itemsKey.Count; i++)
        {
            if (_inventory.CheckObjects(item._itemsKey[i]) < item._itemsCount[i]) return;
        }

        for (int i = 0; i < item._itemsKey.Count; i++)
        {
            _inventory.ReduseObjects(item._itemsKey[i], item._itemsCount[i]);
        }

        _inventory.AddItem(item);
    }
}