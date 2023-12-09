using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static void ChalangeComplete(GameObject JumpPad)
    {
        JumpPad.SetActive(true);
    }

    public static void AddItemsToPlayer(Item item, int count, Inventory inventory)
    {
        Item newItem = new Item();
        newItem.Stack = count;
        inventory.AddItem(newItem);
    }
}