using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShowPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private MoveCamera _camera;
    [SerializeField] private float _stopingTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            StartCoroutine(ShowPoints());
    }

    private IEnumerator ShowPoints()
    {
        Transform playerTransform = _camera.Target;
        for (int i = 0; i < _points.Count; i++)
        {
            _camera.Target = _points[i];
            yield return new WaitForSeconds(_stopingTime);
        }
        _camera.Target = playerTransform;
        Destroy(gameObject);
    }
}