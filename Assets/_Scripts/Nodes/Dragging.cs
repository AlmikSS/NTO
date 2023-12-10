using UnityEngine;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    BlocksMovement _bm;
    NodesLogic _nl;
    private void Awake() {
        _bm = gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<BlocksMovement>();
        _nl = gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<NodesLogic>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _bm.OnInitializeDrag(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _bm.MoveBlock(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _nl.OnDrop(gameObject);
    }
}
