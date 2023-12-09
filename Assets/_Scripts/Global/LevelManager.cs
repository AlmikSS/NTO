using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private int _enemyCount;
    public int DiedEnemyCount;

    private void Update()
    {
        CheckDiedEnemys();
    }

    private void CheckDiedEnemys()
    {
        if (DiedEnemyCount == _enemyCount)
        {
            if (_door != null)
                _door.SetActive(false);
        }
    }
}