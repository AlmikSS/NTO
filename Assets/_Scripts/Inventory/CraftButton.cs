using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] private RawImage _image; // текстура кнопки
    public Item Item; // предмет который крафтиться
    
    public void ChangeColor(Color color) // метод смены цвета текстуры
    {
        _image.color = color; // меняем цвет
    }
}