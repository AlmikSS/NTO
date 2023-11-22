using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodesLogic : MonoBehaviour
{
    [HideInInspector] public List<List<string>> Programm = new List<List<string>>();
    List<List<List<string>>> AllCombinatoins = new List<List<List<string>>>();

    public void OnDrop(GameObject Obj){     
        BlocksMovement _blocksMove = GameObject.Find("Canvas").GetComponent<BlocksMovement>(); 

        if(Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f > 0.3f*Screen.width && Obj.transform.position.x <= Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){
            Obj.transform.SetParent(GameObject.Find("ProgrammView").transform.Find("Viewport/Contented"));//если блок входит в блок программирования, то становится его ребенком
            bool InList = false;
            foreach(List<List<string>> i in AllCombinatoins){
                foreach(List<string> j in i){
                    if(j.Contains(Obj.name)){
                        InList=true;
                        break;}}
            if(InList) break;}

            if(!InList) AllCombinatoins.Add(new List<List<string>>(){new List<string>(){Obj.name}});
        }
        else if(Obj.transform.position.x + Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f >= 0 && Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f <= 0.3f*Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){
            _blocksMove.AllNodes.Remove(Obj);
            foreach(List<List<string>> i in AllCombinatoins){
                foreach(List<string> j in i){
                    if(j.Contains(Obj.name)){
                        j.Remove(Obj.name);}}}
                    
            Destroy(Obj);//если пользователь отпускает блок над нодами, то он исчезает
            return;
        }
        
        
        string _edge =  _blocksMove.DropBlock(Obj);
        GameObject _coll = _blocksMove.Coll;
        int _programsCounter = 0;
        int _strCounter = 0;
        List<int> _strToRemove = new List<int>();
        List<int> _progToRemove = new List<int>();
        foreach(List<List<string>> i in AllCombinatoins){
            _programsCounter++;
            _strCounter = 0;
            _strToRemove = new List<int>();
            foreach(List<string> j in i){
                
                if(j.Contains(Obj.name) && (j.Count != 1 || i.Count != 1)) {
                    j.Remove(Obj.name);
                }

                if(_edge == "rightEdge" && j.Contains(_coll.name)) j.Insert(j.IndexOf(_coll.name),Obj.name);

                else if(_edge == "leftEdge" && j.Contains(_coll.name)) j.Add(Obj.name);

                else if(_edge == "upEdge" && j.Contains(_coll.name)){ 
                    i.Insert(_strCounter,new List<string>(){Obj.name});
                }
                
                else if(_edge == "downEdge" && j.Contains(_coll.name)){
                    i.Add(new List<string>(){Obj.name});
                }
                if(j.Count == 0){
                    _strToRemove.Add(i.IndexOf(j));
                }
            }
            foreach(int r in _strToRemove) i.RemoveAt(r);

            if(i.Count == 0) _progToRemove.Add(AllCombinatoins.IndexOf(i));
            _strCounter++;
        }

        foreach(int r in _progToRemove) AllCombinatoins.RemoveAt(r);
        
        foreach(List<List<string>> i in AllCombinatoins){
            //if(i[0][0].StartsWith("BeginNode") && i[^1][0].StartsWith("EndNode")){
                Debug.Log("[");
                foreach(List<string> j in i){
                    Debug.Log("["+string.Join(", ", j)+"]");
                        
                }
                Debug.Log("]");
            //}
        }
    
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