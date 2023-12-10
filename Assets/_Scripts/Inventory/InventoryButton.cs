using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public Inventory MyInv; // ссылка на инвентарь к которому относиться этот слот/кнопка
    public int MyID; // ID этого слота
    public Item ItemInSlot;
    [SerializeField] private Transform _infoPos;
    [SerializeField] private GameObject _infoPrefab;
    [SerializeField] private List<string> _descriptions = new List<string>();
    private GameObject _info;

    public void Press() // метод нажатия на слот
    {
        MyInv.SelectSlot(MyID); // вызываем метод SelectSlot в инвентаре
    }

    public void OnHoverEnter()
    {
        if (ItemInSlot.ItemType != ItemType.Null)
        {
            _info = Instantiate(_infoPrefab, _infoPos.position, Quaternion.identity);

            switch (ItemInSlot.ItemType)
            {
                case ItemType.ShieldGadjet:
                    _info.GetComponent<ShowInfo>().Show(_descriptions[0]);
                    break;
                case ItemType.DoubleJumpGadjet:
                    _info.GetComponent<ShowInfo>().Show(_descriptions[1]);
                    break;
                case ItemType.Fragment:
                    _info.GetComponent<ShowInfo>().Show(_descriptions[2]);
                    break;
            }
        }
    }

    public void OnHoverExit()
    {
        Destroy(_info);
    }
}