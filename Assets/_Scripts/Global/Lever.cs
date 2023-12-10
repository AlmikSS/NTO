using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(!objects[i].activeSelf);
            }
        }
    }
}