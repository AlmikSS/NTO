using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IDamageable
{
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    public void TakeDamage(int damage)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        
    }

}