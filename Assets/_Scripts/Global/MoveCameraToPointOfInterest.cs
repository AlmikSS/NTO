using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToPointOfInterest : MonoBehaviour
{
    [SerializeField] private List<Transform> _pointsOfInteret = new List<Transform>(); // список точек которые нужно показать
    [SerializeField] private MoveCamera _camera; // ссылка на камеру
    [SerializeField] private float _waitTime; // время, которые ждет камера на точке

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // если в триггер попал игрок
        {
            StartCoroutine(MoveCamera(collision.transform)); // запускаем корутину движения камеры
        }
    }

    public IEnumerator MoveCamera(Transform transform) // метод движения камеры
    {
        for (int i = 0; i < _pointsOfInteret.Count; i++) // проходимся по всем точкам
        {
            _camera.Target = _pointsOfInteret[i]; // меняем цель камеры на точку
            yield return new WaitForSeconds(_waitTime); // ждем
        }
        _camera.Target = transform.GetChild(2); // меняем цель камеры на игрока

        Destroy(gameObject); // удаляем этот объект
    }
}