using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static void ChalangeComplete(GameObject JumpPad)
    {
        JumpPad.SetActive(true);
    }

    public static void AddItemsToPlayer(Item item, int count, Inventory inventory)
    {
        for (int i = 0; i < count; i++)
        {
            inventory.AddItem(item);
        }
    }
}