using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodesLogic : MonoBehaviour
{
    [HideInInspector] public List<string> Programm = new List<string>();
    List<string> AllPositions = new List<string>();
    bool _programmEnd = false;
    public void OnDrop(GameObject Obj){     
        BlocksMovement _blocksMove = GameObject.Find("Canvas").GetComponent<BlocksMovement>(); 

        if(Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f > 0.3f*Screen.width && Obj.transform.position.x <= Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){
            Obj.transform.SetParent(GameObject.Find("ProgrammView").transform.Find("Viewport/Contented"));//если блок входит в блок программирования, то становится его ребенком
        }
        else if(Obj.transform.position.x + Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f >= 0 && Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f <= 0.3f*Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){
            _blocksMove.AllNodes.Remove(Obj);
            Destroy(Obj);//если пользователь отпускает блок над нодами, то он исчезает
            return;
        }
        
        
        string _edge =  _blocksMove.DropBlock(Obj);
        GameObject _coll = _blocksMove.Coll;
        if(_coll!=null && Obj.tag == "BeginNode"){
            Programm = new List<string>
            {
                Obj.name,
                _coll.name
            };
        }
        if(_coll!=null && _coll.tag == "BeginNode"){
            Programm = new List<string>
            {
                _coll.name,
                Obj.name

            };
        }
        else if(Obj.tag == "BeginNode" && _coll==null ) Programm = new List<string>();
        else if(!_programmEnd && _coll!=null && Obj.tag != "BeginNode" && Obj.tag != "EndNode" && Programm.Count > 0 && Programm.Contains(_coll.name) && !Programm.Contains(Obj.name)){
            Programm.Add(Obj.name);
        }
        else if(!_programmEnd && Obj.tag != "BeginNode" && Obj.tag != "EndNode" && Programm.Count > 0 && Programm.Contains(Obj.name)){
            while(Programm.Count > 0 && Programm[Programm.Count-1]!=Obj.name) Programm.RemoveAt(Programm.Count-1);
            Programm.RemoveAt(Programm.Count-1);
        }
        else if(!_programmEnd && _coll!=null && (Obj.tag == "EndNode" || _coll.tag == "EndNode") && Programm.Count > 0){
           Programm.Add(Obj.name);
            _programmEnd = true;
        }
        //foreach(string name in Programm) Debug.Log(name);
    }
}

public class NumberNode
{
    public float Value;
    public NumberNode(float TextValue){
        Value = TextValue;
    }
}
public class StringNode
{
    public string Value;
    public StringNode(string TextValue){
        Value = TextValue;
    }
}
public class VariableNode
{
    public string Name;
    public string Value;
    public VariableNode(string TextName, string TextValue){
        Name = TextName;
        Value = TextValue;
    }

}
public class InputNode
{
    public VariableNode InputVar;
    public InputNode(string TextName, string InputValue){
        InputVar = new(TextName, InputValue);
    }
}
public class OutputNode
{
    public string OutputVar;
    public OutputNode(VariableNode Node){
        OutputVar = Node.Value;
    }
}