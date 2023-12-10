using UnityEngine;

[RequireComponent(typeof(BlocksMovement))]
[RequireComponent(typeof(NodesLogic))]
public class Enable_Disable_Scripts : MonoBehaviour
{
    private void OnEnable() {
        gameObject.GetComponent<BlocksMovement>().enabled=true;
        gameObject.GetComponent<NodesLogic>().enabled=true;
    }
    private void OnDisable() {
        gameObject.GetComponent<BlocksMovement>().enabled=false;
        gameObject.GetComponent<NodesLogic>().enabled=false;
    }
}
