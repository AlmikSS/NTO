using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public Inventory MyInv;
    public int MyID;

    public void Press()
    {
        MyInv.SelectSlot(MyID);
    }
}