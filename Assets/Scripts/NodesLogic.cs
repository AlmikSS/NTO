using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;
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
                        j.Remove(Obj.name); break;}}}
                    
            Destroy(Obj);//если пользователь отпускает блок над нодами, то он исчезает
            return;
        }
        
        
        string _edge =  _blocksMove.DropBlock(Obj);
        GameObject _coll = _blocksMove.Coll;
        int _programsCounter = 0, _strCounter = 0, _strToInsert = -1, _progToInsert = -1, _progToAdd = -1;
        bool _addNew = false;
        List<int> _strToRemove = new List<int>();
        List<int> _progToRemove = new List<int>();
        foreach(List<List<string>> i in AllCombinatoins){

            _strCounter = 0;
            _strToRemove = new List<int>();
            foreach(List<string> j in i){
                
                if(j.Contains(Obj.name) && ((j.Count != 1 || i.Count != 1) || _edge!="")) {
                    j.Remove(Obj.name);
                    if(_edge=="") _addNew = true;
                }

                if(_edge == "leftEdge" && j.Contains(_coll.name)) j.Insert(j.IndexOf(_coll.name),Obj.name);

                else if(_edge == "rightEdge" && j.Contains(_coll.name)) j.Add(Obj.name);

                else if(_edge == "upEdge" && j.Contains(_coll.name)){ 
                    _strToInsert = _strCounter;
                    _progToInsert = _programsCounter;
                }
                
                else if(_edge == "downEdge" && j.Contains(_coll.name)){
                    _progToAdd = _programsCounter;
                }
                if(j.Count == 0){
                    _strToRemove.Add(i.IndexOf(j));
                }
                _strCounter++;
            }
            foreach(int r in _strToRemove) i.RemoveAt(r);

            if(i.Count == 0) _progToRemove.Add(AllCombinatoins.IndexOf(i));
            
            _programsCounter++;
        }

        if( _progToAdd >= 0){ 
            AllCombinatoins[_progToAdd].Add(new List<string>(){Obj.name});
            _progToAdd = -1;
        }
        if( _strToInsert >= 0 && _progToInsert >= 0){
             AllCombinatoins[_progToInsert].Insert(_strToInsert,new List<string>(){Obj.name});
             _strToInsert = -1;
             _progToInsert = -1;
        }
        if(_addNew) AllCombinatoins.Add(new List<List<string>>(){new List<string>(){Obj.name}});

        foreach(int r in _progToRemove) AllCombinatoins.RemoveAt(r);
        
    
    }

    public void StartProgramm(){

        List<VariableNode> Variables = new();

        foreach(List<List<string>> i in AllCombinatoins){
            if(i[0][0].StartsWith("BeginNode") && i[^1][0].StartsWith("EndNode")){
                foreach(List<string> str in i){
                    Debug.Log("["+string.Join(", ", str)+"]");

                    if(str[0].StartsWith("VariableNode")){
                        GameObject obj = GameObject.Find(str[0]);
                        string _txt = obj.transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        string _type;
                        string _value = Assinment(AllCombinatoins.IndexOf(i),i.IndexOf(str));
                        if(float.TryParse(_value,out var number)) _type = "float";
                        else _type = "string";
                        Variables.Add(new VariableNode(_txt,_value, _type));
                        Debug.Log(_txt+" "+_value+" "+ _type);
                    }
                }
            }
        }
        string Assinment(int i, int str){
            string _value = "";
            int _nodeCount = 0;
            List<string> _currentStr = AllCombinatoins[i][str];
            if(_currentStr.Count>1 && _currentStr[1].StartsWith("AssignmentNode")){
                foreach(string node in _currentStr){

                    if((node.StartsWith("NumberNode") || node.StartsWith("StringNode")) &&  _currentStr[_nodeCount-1].StartsWith("AssignmentNode")){
                        string _txt = GameObject.Find(node).transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        _value =_txt;
                    }

                    else if(node.StartsWith("VariableNode") && _nodeCount>1 && _currentStr[_nodeCount-1].StartsWith("AssignmentNode")){
                        string _txt = GameObject.Find(node).transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        _value += Variables.Find(x => x.Name ==  _txt).Value;
                    }
                    else if(GameObject.Find(node).tag=="MathNodes" && _currentStr.Count>=_nodeCount+1 && (_currentStr[_nodeCount-1].StartsWith("StringNode") || _currentStr[_nodeCount-1].StartsWith("NumberNode") || _currentStr[_nodeCount-1].StartsWith("VariableNode")) && (_currentStr[_nodeCount+1].StartsWith("StringNode") || _currentStr[_nodeCount+1].StartsWith("NumberNode") || _currentStr[_nodeCount+1].StartsWith("VariableNode"))){

                        string _value2 = "";
                        string _lastTxt = GameObject.Find(_currentStr[_nodeCount-1]).transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        string _nextTxt = GameObject.Find(_currentStr[_nodeCount+1]).transform.GetChild(1).GetComponent<TMP_InputField>().text;

                        if((_currentStr[_nodeCount-1].StartsWith("NumberNode") || Variables.Exists(x => x.Name == _lastTxt && x.Type == "float")) && _currentStr[_nodeCount+1].StartsWith("NumberNode")){
                            _value2 = _nextTxt;
                        }
                        else if((_currentStr[_nodeCount-1].StartsWith("NumberNode") || Variables.Exists(x => x.Name == _lastTxt && x.Type == "float")) && Variables.Exists(x => x.Name == _nextTxt && x.Type == "float")){
                            _value2 = Variables.Find(x => x.Name == _nextTxt).Value;
                        }
                        else if((_currentStr[_nodeCount-1].StartsWith("StringNode") || Variables.Exists(x => x.Name == _lastTxt && x.Type == "string")) && _currentStr[_nodeCount+1].StartsWith("StringNode") ){
                            _value2 = _nextTxt;
                        }
                        else if((_currentStr[_nodeCount-1].StartsWith("StringNode") || Variables.Exists(x => x.Name == _lastTxt && x.Type == "string")) && Variables.Exists(x => x.Name == _nextTxt && x.Type == "string")) {
                            _value2 = Variables.Find(x => x.Name == _nextTxt).Value;
                        }
                        else return "";

                        if(float.TryParse(_value,out var number1) && float.TryParse(_value2,out var number2)){
                            if(node.StartsWith("AdditionNode")) _value=(float.Parse(_value) + float.Parse(_value2)).ToString();
                            else if(node.StartsWith("SubstractionNode")) _value=(float.Parse(_value) - float.Parse(_value2)).ToString();
                            else if(node.StartsWith("DevisionNode")) _value=(float.Parse(_value) / float.Parse(_value2)).ToString();
                            else if(node.StartsWith("MultiplicationNode")) _value=(float.Parse(_value) * float.Parse(_value2)).ToString();
                            else if(node.StartsWith("RemainderNode")) _value=(float.Parse(_value) % float.Parse(_value2)).ToString();
                            else if(int.TryParse(_value,out var number3) && int.TryParse(_value2,out var number4) && node.StartsWith("IntDevisionNode")) {
                                _value=(int.Parse(_value)/int.Parse(_value2)).ToString();
                            }
                        }
                        else{
                            if(node.StartsWith("AdditionNode")) _value+=_value2;
                            else return "";
                        }
                    }
                    
                    _nodeCount++;
                }
            }
            return _value;
        }
    }
}

/*public class NumberNode
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
}*/
public class VariableNode
{
    public string Name;
    public string Value;
    public string Type;
    public VariableNode(string TextName, string TextValue, string TextType){
        Name = TextName;
        Value = TextValue;
        Type = TextType;
    }

}
public class InputNode
{
    public VariableNode InputVar;
    public InputNode(string TextName, string InputValue,  string TextType){
        InputVar = new(TextName, InputValue, TextType);
    }
}
public class OutputNode
{
    public string OutputVar;
    public OutputNode(VariableNode Node){
        OutputVar = Node.Value;
    }
}