using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private bool _freezeCamera;
    [SerializeField] private MoveCamera _moveCamera;
    [SerializeField] private Transform _focesPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_freezeCamera)
                {
                    _moveCamera.Target = _focesPoint;
                }
            }
            rb.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
        }
    }
}