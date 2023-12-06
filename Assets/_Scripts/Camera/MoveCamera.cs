using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // ���� �������� ������������ ������
    [SerializeField] private float _stopInPointTime;
    public Transform Target; // ���� Transform ����

    private void FixedUpdate()
    {
        Vector3 target = new Vector3 // ������� ������ target
        {
            x = Target.position.x,
            y = Target.position.y + 1,
            z = Target.position.z - 10,
        };
        // ������� ������ pos ������� ����� ����� ���������� ������� ������
        Vector3 pos = Vector3.Lerp(transform.position, target, _moveSpeed * Time.deltaTime); 
        transform.position = pos; // ������������� ������ � pos
    }
}