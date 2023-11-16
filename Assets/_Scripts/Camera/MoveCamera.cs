using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        Vector3 target = new Vector3
        {
            x = _playerTransform.position.x,
            y = _playerTransform.position.y + 1,
            z = _playerTransform.position.z - 10,
        };

        Vector3 pos = Vector3.Lerp(transform.position, target, _moveSpeed * Time.deltaTime);
        transform.position = pos;
    }
}