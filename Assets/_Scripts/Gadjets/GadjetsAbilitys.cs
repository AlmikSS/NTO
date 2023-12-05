using System.Collections;
using UnityEngine;

public class GadjetsAbilitys : MonoBehaviour
{
    [SerializeField] private float _doubleJumpCulDown;
    [SerializeField] private float _shieldTime;
    [SerializeField] private float _shieldCulDown;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Transform _shiledSpawnPoint;
    public bool ReadyToDoubleJump = true;
    public bool ReadyToInstShield = true;

    public IEnumerator DoubleJump(PlayerController controller)
    {
        if (!controller.Grounded)
        {
            controller.Jump(true);
            ReadyToDoubleJump = false;
            yield return new WaitForSeconds(_doubleJumpCulDown);
            ReadyToDoubleJump = true;
        }
    }

    public IEnumerator ShieldGadjet(Transform transform)
    {
        GameObject shield = Instantiate(_shieldPrefab, _shiledSpawnPoint.position, Quaternion.identity, transform);
        ReadyToInstShield = false;
        yield return new WaitForSeconds(_shieldTime);
        Destroy(shield.gameObject);
        yield return new WaitForSeconds(_shieldCulDown);
        ReadyToInstShield = true;
    }
}