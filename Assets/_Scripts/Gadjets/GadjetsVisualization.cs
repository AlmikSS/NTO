using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadjetsVisualization : MonoBehaviour
{
    [SerializeField] private GadjetsInventory _inventory;
    [SerializeField] private List<RawImage> _gadjetsImage = new List<RawImage>();

    public void Redraw()
    {
        for (int i = 0; i < _gadjetsImage.Count; i++)
        {
            _gadjetsImage[i].texture = _inventory.Items[i].Image;
        }
    }
}