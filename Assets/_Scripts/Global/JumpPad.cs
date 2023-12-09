using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _force;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
        }
    }
}