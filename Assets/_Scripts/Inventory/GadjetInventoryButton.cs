using UnityEngine;

public class GadjetInventoryButton : MonoBehaviour
{
    public GadjetsInventory MyInv; // ссылка на инвентарь к которому относиться этот слот/кнопка
    public int MyID; // ID этого слота

    public void Press() // метод нажатия на слот
    {
        MyInv.SelectSlot(MyID); // вызываем метод SelectSlot в инвентаре
    }
}