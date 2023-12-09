using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static void ChalangeComplete(GameObject JumpPad)
    {
        JumpPad.SetActive(true);
    }
}