using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlocksMovement : MonoBehaviour
{

    [SerializeField] GameObject[] AllNodes = new GameObject[1];//все ноды на уровне
    [SerializeField] float ClueDistnce = 15f;//дистанция склеивания нодов
    public void MoveBlock(Transform obj){
        obj.position = Input.mousePosition; 
        //когда пользователь начинает двигать блок он копирует позицию мыши
    }
    public void DropBlock(GameObject Obj){//когда пользователь отпускает нод
        List<float> _distances = new List<float>();//создаем пустой массив для дистанций
        GameObject _coll = Obj;// создаем переменнуб для ближайшего объекта 
        float _minDist = 1000000000000000000f;// задаем минимум
        foreach (GameObject i in AllNodes)//перебираем все ноды
        {
            float _dist = Vector2.Distance(i.transform.position, Obj.transform.position);//находим дистанцию до ближайшего нода
            if (i != Obj){// если не равен передвигаемому ноду, то добавляем в дистанции и сравниваем с минимумом
                _distances.Add(_dist);
                if( _dist<_minDist){
                    _minDist = _dist;
                    _coll = i;
                }
            }
           
        }
        
        Vector2 _collPos = _coll.transform.position; //позиция сталкивающегося блока
        Vector2 _objPos = Obj.transform.position; //позиция передвигаемого блока
        Vector2 _collSize = _coll.GetComponent<RectTransform>().sizeDelta; // размеры сталкивающегося блока
        Vector2 _objSize = Obj.GetComponent<RectTransform>().sizeDelta; // размеры передвигаемого блока

        float _downEdge = Mathf.Abs(_collPos.y-(_objPos.y+_objSize.y)); //расчитывает абсолютную разницу между верхней границей передвигаемого блока и нижней границей сталкивающегося
        float _upEdge = Mathf.Abs(_collPos.y+_collSize.y-_objPos.y); //расчитывает абсолютную разницу между нижней границей передвигаемого блока и верхней границей сталкивающегося
        float _leftEdge = Mathf.Abs(_collPos.x-(_objPos.x+_objSize.x)); //расчитывает абсолютную разницу между правой границей передвигаемого блока и левой границей сталкивающегося
        float _rightEdge = Mathf.Abs(_collPos.x+_collSize.x-_objPos.x); //расчитывает абсолютную разницу между левой границей передвигаемого блока и правой границей сталкивающегося

        if(Mathf.Min(_downEdge, _upEdge) == _downEdge && _objPos.y >= _collPos.y-_collSize.y-ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){// проверяем если он касает нижней стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x,_collPos.y-_collSize.y);
            Debug.Log("downEgde");
        }
        else if(Mathf.Min(_downEdge, _upEdge) == _upEdge && _objPos.y <= _collPos.y+_collSize.y+ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){// проверяем если он касает верхней стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x,_collPos.y+_collSize.y);
            Debug.Log("upEdge");
        }
        else if(Mathf.Min(_leftEdge, _rightEdge) == _leftEdge && _objPos.x >= _collPos.x-_collSize.x-ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){// проверяем если он касает левой стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x-_collSize.x,_collPos.y);
            Debug.Log("leftEdge");
        }
        else if(Mathf.Min(_leftEdge, _rightEdge) == _rightEdge && _objPos.x <= _collPos.x+_collSize.x+ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){// проверяем если он касает правой стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x+_collSize.x,_collPos.y);
            Debug.Log("rightEdge");
        }
    
    }
    

   
}
