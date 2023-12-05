using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlocksMovement : MonoBehaviour
{

    public List<GameObject> AllNodes = new List<GameObject>();//все ноды на уровне
    [SerializeField] float ClueDistnce = 15f;//дистанция склеивания нодов
    [HideInInspector] public GameObject Coll;// создаем публичную переменную для ближайшего объекта 
    private NodesLogic _nodesLogic; //записываем скрипт логики в переменную 
    private Input _playerInput;

    private void OnEnable()
    {
        _playerInput = new Input();
        _playerInput.Enable();
        _nodesLogic = GetComponent<NodesLogic>();
    }
    private void OnDisable()
    {
        _playerInput.Disable();
    }
    
    public void OnInitializeDrag(GameObject Obj){//когда пользователь начинает двигать нод
        if(Obj.transform.parent.parent.parent.name == "NodesView"){//проверяем находится ли он в теле нодов
            GameObject _newObj = Instantiate(Obj, new Vector2(Obj.transform.position.x, Obj.transform.position.y),  Quaternion.identity, Obj.transform.parent);
            AllNodes.Add(_newObj);//создаем копию и добавляем в список ко всем
        }
    }

    public void MoveBlock(GameObject Obj){//когда пользователь двигает блок
        if(Obj.transform.position.x >= 0 && Obj.transform.position.x <= Screen.width && Obj.transform.position.y>= 0 && Obj.transform.position.y <= Screen.height){
            Obj.transform.SetParent(GameObject.Find("Canvas").transform);
            Obj.transform.position = _playerInput.UI.MousePosition.ReadValue<Vector2>();//если блок находится в пределах экрана и не выходит за границы, то копирует позицию мыши
        }
        else{
            AllNodes.Remove(Obj.gameObject);
            bool _del = false;
            foreach(List<List<string>> i in _nodesLogic.AllCombinatoins){
                foreach(List<string> j in i){
                    if(j.Contains(Obj.name)){

                        if(j.IndexOf(Obj.name)+1<j.Count){
                            _nodesLogic.AllCombinatoins.Add(new List<List<string>>(){j.GetRange(j.IndexOf(Obj.name)+1, j.Count-j.IndexOf(Obj.name)-1).ToList()});
                            j.RemoveRange(j.IndexOf(Obj.name)+1, j.Count-j.IndexOf(Obj.name)-1);
                        }

                        if(i.IndexOf(j)+1< i.Count && j.IndexOf(Obj.name)==0){
                            _nodesLogic.AllCombinatoins.Add(i.GetRange(i.IndexOf(j)+1, i.Count-i.IndexOf(j)-1).ToList());
                            i.RemoveRange(i.IndexOf(j)+1, i.Count-i.IndexOf(j)-1);
                        }
                        j.Remove(Obj.name);
                        _del = true;
                        break;
                    }
                }
                if(_del) break;
            } // удаляем из списка позиций всех нодов

            Destroy(Obj); //удаляем
            return;
        }
    }

     public string DropBlock(GameObject Obj){//вызывается из NodesLogic

        
        Vector2 _objPos = Obj.transform.position, _collSize, _collPos; //позиция текущего нода
        
        Vector2 _objSize = new Vector2(Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f),Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)); // размеры текущего нода
        
        float _minDist = 1000000000000000000f;// задаем минимум
        foreach (GameObject i in AllNodes)//перебираем все ноды
        {
            //находим дистанцию до нода
            if (i != Obj && Obj.transform.parent.parent.parent.name == i.transform.parent.parent.parent.name){// если не равен передвигаемому ноду и общий родитель
                _collSize = new Vector2(i.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f),i.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)); // размеры проверяемого нода

                float _leftDist = Vector2.Distance(new Vector2(i.transform.position.x-_collSize.x*0.5f, i.transform.position.y), new Vector2(Obj.transform.position.x+_objSize.x*0.5f, Obj.transform.position.y)); //дистанция между левой стороной проверяемого нода и правой текущего нода
                float _rightDist = Vector2.Distance(new Vector2(i.transform.position.x+_collSize.x*0.5f, i.transform.position.y), new Vector2(Obj.transform.position.x-_objSize.x*0.5f, Obj.transform.position.y));//дистанция между правой стороной проверяемого нода и леовй текущего нода
                float _upDist = Vector2.Distance(new Vector2(i.transform.position.x,i.transform.position.y+_collSize.y*0.5f), new Vector2(Obj.transform.position.x, Obj.transform.position.y-_objSize.y*0.5f));//дистанция между верхней стороной проверяемого нода и нижней текущего нода
                float _downDist = Vector2.Distance(new Vector2(i.transform.position.x,i.transform.position.y-_collSize.y*0.5f), new Vector2(Obj.transform.position.x, Obj.transform.position.y+_objSize.y*0.5f));//дистанция между нижней стороной проверяемого нода и верхней текущего нода
                if( Mathf.Min(_leftDist,_rightDist,_upDist,_downDist)<_minDist){
                    _minDist = Mathf.Min(_leftDist,_rightDist,_upDist,_downDist);
                    Coll = i;//находим минимум и присваиваем переменной для сталкивающегося с текущим нодом значение проверяемого
                }
                
            }
           
        }
        if(Coll == null){
            return "";
        }
        _collSize = new Vector2(Coll.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f),Coll.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)); // размеры сталкивающегося блока
        _collPos = Coll.transform.position; //позиция сталкивающегося блока

        bool _canRight = false, _canLeft = false, _canUp = false, _canDown = false;//переменные, разрешающие приклеивание нодов по данному направлению
        List<string> _objCanClue = new(), _collCanClue = new();//массивы, в которых хранятся все возможные варианты склеивания с этим нодом

        /////////////////Для передвигаемого нода, для каждого нода разные варианты склейки
        if(Obj.tag=="BeginNode") _objCanClue.Add("down");
        else if(Obj.tag=="VariableNode"){ _objCanClue.Add("up"); _objCanClue.Add("down"); _objCanClue.Add("right"); _objCanClue.Add("left");}
        else if(Obj.tag=="NumberNode" || Obj.tag=="LogicalNodes" || Obj.tag=="StringNode" || Obj.tag=="MathNodes"){_objCanClue.Add("right"); _objCanClue.Add("left");}
        else if(Obj.tag=="IfNode" || Obj.tag=="ElseNode" || Obj.tag=="WhileNode"){ _objCanClue.Add("up"); _objCanClue.Add("down"); _objCanClue.Add("right");}
        else if(Obj.tag=="EmptyNode" || Obj.tag=="InputNode" || Obj.tag=="OutputNode" || Obj.tag=="ElementParseNode") {_objCanClue.Add("up"); _objCanClue.Add("down");}
        else if(Obj.tag=="EndNode") _objCanClue.Add("up");

        /////////////////Для сталкивающегося нода, для каждого нода разные варианты склейки
        if(Coll.tag=="BeginNode") _collCanClue.Add("down");
        else if(Coll.tag=="VariableNode"){ _collCanClue.Add("up"); _collCanClue.Add("down"); _collCanClue.Add("right"); _collCanClue.Add("left");}
        else if(Coll.tag=="NumberNode" || Coll.tag=="LogicalNodes" || Coll.tag=="StringNode" || Coll.tag=="MathNodes"){_collCanClue.Add("right"); _collCanClue.Add("left");}
        else if(Coll.tag=="IfNode" || Coll.tag=="ElseNode" || Coll.tag=="WhileNode"){ _collCanClue.Add("up"); _collCanClue.Add("down"); _collCanClue.Add("right");}
        else if(Coll.tag=="EmptyNode" || Coll.tag=="InputNode" || Coll.tag=="OutputNode" || Coll.tag=="ElementParseNode") { _collCanClue.Add("up"); _collCanClue.Add("down");}
        else if(Coll.tag=="EndNode") _collCanClue.Add("up");

 
        /////////////////Определение областей склеивания
        bool _contLeft = true, _contRight = true, _contDown = true, _contUp = true;
        foreach(List<List<string>> i in _nodesLogic.AllCombinatoins){
            foreach(List<string> j in i){
                if(j.Contains(Coll.name) && j.IndexOf(Coll.name)<j.Count-1){
                    _contRight = false;
                }
                if(j.Contains(Coll.name) && j.IndexOf(Coll.name)>0){
                    _contLeft = false;
                }
                if(j.Contains(Coll.name) && i.IndexOf(j)<i.Count-1){
                    _contDown = false;
                }
                if(j.Contains(Coll.name) && i.IndexOf(j)>0){
                    _contUp = false;
                }
            }
        }
        if(_objCanClue.Contains("right") && _collCanClue.Contains("left") && _contLeft) _canLeft = true;
        if(_objCanClue.Contains("left") && _collCanClue.Contains("right") && _contRight) _canRight = true;
        if(_objCanClue.Contains("up") && _collCanClue.Contains("down") && _contDown) _canDown = true;
        if(_objCanClue.Contains("down") && _collCanClue.Contains("up") && _contUp) _canUp = true;
        

        
        float _downEdge = Mathf.Abs(_collPos.y-(_objPos.y+_objSize.y)); //расчитывает абсолютную разницу между верхней границей передвигаемого блока и нижней границей сталкивающегося
        float _upEdge = Mathf.Abs(_collPos.y+_collSize.y-_objPos.y); //расчитывает абсолютную разницу между нижней границей передвигаемого блока и верхней границей сталкивающегося
        float _leftEdge = Mathf.Abs(_collPos.x-(_objPos.x+_objSize.x)); //расчитывает абсолютную разницу между правой границей передвигаемого блока и левой границей сталкивающегося
        float _rightEdge = Mathf.Abs(_collPos.x+_collSize.x-_objPos.x); //расчитывает абсолютную разницу между левой границей передвигаемого блока и правой границей сталкивающегося

        if(_canDown && Mathf.Min(_downEdge, _upEdge) == _downEdge && _objPos.y >= _collPos.y-_collSize.y*0.5f-_objSize.y*0.5f-ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){// проверяем если он касает нижней стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x,_collPos.y-_collSize.y*0.5f-_objSize.y*0.5f);
            return "downEdge";
        }
        else if(_canUp && Mathf.Min(_downEdge, _upEdge) == _upEdge && _objPos.y <= _collPos.y+_collSize.y*0.5f+_objSize.y*0.5f+ClueDistnce && Mathf.Abs(_leftEdge-_rightEdge) <= ClueDistnce){// проверяем если он касает верхней стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x,_collPos.y+_collSize.y*0.5f+_objSize.y*0.5f);
            return "upEdge";
        }
        else if(_canLeft && Mathf.Min(_leftEdge, _rightEdge) == _leftEdge && _objPos.x >= _collPos.x-_collSize.x*0.5f-_objSize.x*0.5f-ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){// проверяем если он касает левой стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x-_collSize.x*0.5f-_objSize.x*0.5f,_collPos.y);
            return "leftEdge";
        }
        else if(_canRight && Mathf.Min(_leftEdge, _rightEdge) == _rightEdge && _objPos.x <= _collPos.x+_collSize.x*0.5f+_objSize.x*0.5f+ClueDistnce && Mathf.Abs(_downEdge- _upEdge) <= ClueDistnce){// проверяем если он касает правой стороны и приклеиваем
            Obj.transform.position = new Vector2(_collPos.x+_collSize.x*0.5f+_objSize.x*0.5f,_collPos.y);
            return "rightEdge";
        }
        else{Coll = null; return "";}// если ни с кем не контактирует, то обнуляем переменную сталкивающего нода
    
    }
    

   
}
