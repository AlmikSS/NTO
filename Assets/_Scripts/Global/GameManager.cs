using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static void ChalangeComplete(GameObject JumpPad, GameObject sign)
    {
        JumpPad.SetActive(true);
        sign.SetActive(false);
    }

    public static void AddItemsToPlayer(Item item, int count, Inventory inventory)
    {
        for (int i = 0; i < count; i++)
        {
            inventory.AddItem(item);
        }
    }
}