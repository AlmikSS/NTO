using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToPointOfInterest : MonoBehaviour
{
    [SerializeField] private List<Transform> _pointsOfInteret = new List<Transform>(); // ������ ����� ������� ����� ��������
    [SerializeField] private MoveCamera _camera; // ������ �� ������
    [SerializeField] private float _waitTime; // �����, ������� ���� ������ �� �����

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) // ���� � ������� ����� �����
        {
            StartCoroutine(MoveCamera(collision.transform)); // ��������� �������� �������� ������
        }
    }

    public IEnumerator MoveCamera(Transform transform) // ����� �������� ������
    {
        for (int i = 0; i < _pointsOfInteret.Count; i++) // ���������� �� ���� ������
        {
            _camera.Target = _pointsOfInteret[i]; // ������ ���� ������ �� �����
            yield return new WaitForSeconds(_waitTime); // ����
        }
        _camera.Target = transform.GetChild(2); // ������ ���� ������ �� ������

        Destroy(gameObject); // ������� ���� ������
    }
}