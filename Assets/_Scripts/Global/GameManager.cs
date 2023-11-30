using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void Awake()
    {
        _inventory.Inizialize();
    }
}