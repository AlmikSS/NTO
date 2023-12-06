using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // поле скорости передвижения камеры
    [SerializeField] private float _stopInPointTime;
    public Transform Target; // поле Transform цели

    private void FixedUpdate()
    {
        Vector3 target = new Vector3 // создаем вектор target
        {
            x = Target.position.x,
            y = Target.position.y + 1,
            z = Target.position.z - 10,
        };
        // создаем вектор pos который равен новой сглаженной позиции камеры
        Vector3 pos = Vector3.Lerp(transform.position, target, _moveSpeed * Time.deltaTime); 
        transform.position = pos; // устанавливаем камеру в pos
    }
}