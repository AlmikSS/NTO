using TMPro;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    public void Show(string description)
    {
        _text.text = description;
    }
}