
using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider
    /// and is still holding down the mouse.
    /// </summary>
    [SerializeField] GameObject[] AllNodes = new GameObject[1];
    [SerializeField] float ClueDistnce = 15f;
    public void MoveBlock(Transform obj){
        obj.position = Input.mousePosition; 
        //когда пользователь начинает двигать блок он копирует позицию мыши
    }
    public void DropBlock(GameObject Obj){
        foreach( GameObject _coll in AllNodes){
            if(_coll != Obj){
                Vector2 _collPos = _coll.transform.position; //позиция сталкивающегося блока
                Vector2 _objPos = Obj.transform.position; //позиция передвигаемого блока
                Vector2 _collSize = _coll.GetComponent<RectTransform>().sizeDelta; // размеры сталкивающегося блока
                Vector2 _objSize = Obj.GetComponent<RectTransform>().sizeDelta; // размеры передвигаемого блока

                float _downEdge = Mathf.Abs(_collPos.y-(_objPos.y+_objSize.y)); //расчитывает абсолютную разницу между верхней границей передвигаемого блока и нижней границей сталкивающегося
                float _upEdge = Mathf.Abs((_collPos.y+_collSize.y)-_objPos.y); //расчитывает абсолютную разницу между нижней границей передвигаемого блока и верхней границей сталкивающегося
                float _leftEdge = Mathf.Abs(_collPos.x-(_objPos.x+_objSize.x)); //расчитывает абсолютную разницу между правой границей передвигаемого блока и левой границей сталкивающегося
                float _rightEdge = Mathf.Abs((_collPos.x+_collSize.x)-_objPos.x); //расчитывает абсолютную разницу между левой границей передвигаемого блока и правой границей сталкивающегося

                if(Mathf.Min(_downEdge, _upEdge) == _downEdge && _objPos.y >= _collPos.y-_collSize.y-ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){
                    Obj.transform.position = new Vector2(_collPos.x,_collPos.y-_collSize.y);
                    Debug.Log("downEgde");
                }
                else if(Mathf.Min(_downEdge, _upEdge) == _upEdge && _objPos.y <= _collPos.y+_collSize.y+ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){
                    Obj.transform.position = new Vector2(_collPos.x,_collPos.y+_collSize.y);
                    Debug.Log("upEdge");
                }
                else if(Mathf.Min(_leftEdge, _rightEdge) == _leftEdge && _objPos.x >= _collPos.x-_collSize.x-ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){
                    Obj.transform.position = new Vector2(_collPos.x-_collSize.x,_collPos.y);
                    Debug.Log("leftEdge");
                }
                else if(Mathf.Min(_leftEdge, _rightEdge) == _rightEdge && _objPos.x <= _collPos.x+_collSize.x+ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){
                    Obj.transform.position = new Vector2(_collPos.x+_collSize.x,_collPos.y);
                    Debug.Log("rightEdge");
                }
            }
        }
    }
    

   
}
