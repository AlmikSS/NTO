using UnityEngine;

public class ShowLearning : MonoBehaviour
{
    public void Show(GameObject obj){
        obj.SetActive(true);
    }
    public void Hide(GameObject obj){
        obj.SetActive(false);
    }
}
