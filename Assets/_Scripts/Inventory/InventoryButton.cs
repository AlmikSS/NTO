using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public Inventory MyInv; // ссылка на инвентарь к которому относиться этот слот/кнопка
    public int MyID; // ID этого слота
    public Item ItemInSlot;
    [SerializeField] private GameObject _image;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private List<string> _descriptions = new List<string>();

    public void Press() // метод нажатия на слот
    {
        MyInv.SelectSlot(MyID); // вызываем метод SelectSlot в инвентаре
    }

    public void OnHoverEnter()
    {
        if (ItemInSlot.ItemType != ItemType.Null)
        {
            _image.SetActive(true);

            switch (ItemInSlot.ItemType)
            {
                case ItemType.ShieldGadjet:
                    _infoText.text = _descriptions[0];
                    break;
                case ItemType.DoubleJumpGadjet:
                    _infoText.text = _descriptions[1];
                    break;
                case ItemType.Fragment:
                    _infoText.text = _descriptions[2];
                    break;
            }
        }
    }
}