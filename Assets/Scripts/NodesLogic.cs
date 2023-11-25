using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NodesLogic : MonoBehaviour
{
    [HideInInspector] public List<List<string>> Programm = new List<List<string>>(); // список позиций нодов для конечной программы
    List<List<List<string>>> AllCombinatoins = new List<List<List<string>>>(); // список позиций всех нодов по группам
    
    [SerializeField] private string InputText;
    [SerializeField] private string InputValue;
    [SerializeField] private string Answer;
    [SerializeField] TMP_Text InputField;
    [SerializeField] TMP_Text OutputField;
    private void OnEnable() {
        InputField.text = InputText;
    }
    public void OnDrop(GameObject Obj){ // при бросании
        BlocksMovement _blocksMove = GameObject.Find("Canvas").GetComponent<BlocksMovement>(); //записываем скрипт передвижения в переменную 

        if(Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f > 0.3f*Screen.width && Obj.transform.position.x <= Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){ //проверяем входит ли в блок программирования при бросании
            Obj.transform.SetParent(GameObject.Find("ProgrammView").transform.Find("Viewport/Contented")); // становится ребенком блока программирования
            bool InList = false;
            foreach(List<List<string>> i in AllCombinatoins){
                foreach(List<string> j in i){
                    if(j.Contains(Obj.name)){
                        InList=true;
                        break;}}
            if(InList) break;} // перебираем список со всеми комбинациями, чтобы понять есть ли нод там

            if(!InList) AllCombinatoins.Add(new List<List<string>>(){new List<string>(){Obj.name}}); //если нет, то добавляем
        }
        else if(Obj.transform.position.x + Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f >= 0 && Obj.transform.position.x - Obj.GetComponent<RectTransform>().sizeDelta.x*(Screen.width/1920f)*0.5f <= 0.3f*Screen.width && Obj.transform.position.y + Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f >= 0 && Obj.transform.position.y - Obj.GetComponent<RectTransform>().sizeDelta.y*(Screen.height/1080f)*0.5f <= Screen.height){//проверяем если пользователь отпускает нод над телом с нодами
            _blocksMove.AllNodes.Remove(Obj); // удаляем из списка всех нодов
            foreach(List<List<string>> i in AllCombinatoins){
                foreach(List<string> j in i){
                    if(j.Contains(Obj.name)){
                        j.Remove(Obj.name); break;}}} // удаляем из списка позиций всех нодов
            foreach(List<string> i in Programm){
                if(i.Contains(Obj.name)){
                    i.Remove(Obj.name); break;}} // удаляем из списка позиций программы    
            Destroy(Obj); //удаляем
            return;
        }
        
        
        string _edge =  _blocksMove.DropBlock(Obj); //вызываем функцию склеивания и получаем грань склейки
        GameObject _coll = _blocksMove.Coll; //получаем сталкивающийся нод
        int _programsCounter = 0, _strCounter = 0, _strToInsert = -1, _progToInsert = -1, _progToAdd = -1; // переменные для счетчика групп нодов, для счетчика строк в группах, для индексов позиции в строке для вставки, строки для вставки, строки для добавления
        bool _addNew = false;//переменная для добавления новой группы
        List<int> _strToRemove = new List<int>(); //список для удаления пустых строк
        List<int> _progToRemove = new List<int>(); //список для удаления пустых групп
        foreach(List<List<string>> i in AllCombinatoins){ //перебираем все группы

            _strCounter = 0;//обнуляем счетчик строк
            _strToRemove = new List<int>(); //и обнуляем список пустых строк
            foreach(List<string> j in i){//перебираем строки
                
                if(j.Contains(Obj.name) && ((j.Count != 1 || i.Count != 1) || _edge!="")) {
                    j.Remove(Obj.name); //если в строке есть текущий нод, но он не один или с кем-то стокнулся, то удаляем
                    if(_edge=="") _addNew = true; //если же ни с кем не столкнулся, то создаем новый
                }

                if(_edge == "leftEdge" && j.Contains(_coll.name)) j.Insert(j.IndexOf(_coll.name),Obj.name); // если грань склейки левая, то вставляем слева

                else if(_edge == "rightEdge" && j.Contains(_coll.name)) j.Add(Obj.name); // если грань склейки правая, то добавяем справа

                else if(_edge == "upEdge" && j.Contains(_coll.name)){//если верхняя грань, то вставлем на строчку выше
                    _strToInsert = _strCounter;
                    _progToInsert = _programsCounter;
                }
                
                else if(_edge == "downEdge" && j.Contains(_coll.name)){//если нижняя грань, то добавляем строчку снизу и вставляем туда
                    _progToAdd = _programsCounter;
                }
                if(j.Count == 0){//если длинна строки 0, то добавляем в список пустых строк
                    _strToRemove.Add(i.IndexOf(j));
                }
                _strCounter++;
            }
            foreach(int r in _strToRemove) i.RemoveAt(r);//удаляем все строки из списка

            if(i.Count == 0) _progToRemove.Add(AllCombinatoins.IndexOf(i));//если длинна группы 0, то добавляем в список пустых групп
            
            _programsCounter++;
        }

        if( _progToAdd >= 0){ //если нужно добавить строку, то добавляем после сталкивающегося нода
            AllCombinatoins[_progToAdd].Add(new List<string>(){Obj.name});
            _progToAdd = -1;
        }
        if( _strToInsert >= 0 && _progToInsert >= 0){//если нужно вставить строку, то перед сталкивающим нодом
             AllCombinatoins[_progToInsert].Insert(_strToInsert,new List<string>(){Obj.name});
             _strToInsert = -1;
             _progToInsert = -1;
        }
        if(_addNew) AllCombinatoins.Add(new List<List<string>>(){new List<string>(){Obj.name}});//если нужно добавить

        foreach(int r in _progToRemove) AllCombinatoins.RemoveAt(r);//удаляем все группы из списка
        
    
    }

    public void StartProgramm(){

        List<VariableNode> Variables = new();

        foreach(List<List<string>> i in AllCombinatoins){
            if(i[0][0].StartsWith("BeginNode") && i[^1][0].StartsWith("EndNode")){
                foreach(List<string> str in i){
                    Debug.Log("["+string.Join(", ", str)+"]");

                    if(str[0].StartsWith("VariableNode")){
                        GameObject obj = GameObject.Find(str[0]);
                        string _name = obj.transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        string _type;
                        string _value = Assignment(AllCombinatoins.IndexOf(i),i.IndexOf(str));
                        if(float.TryParse(_value,out var number)) _type = "float";
                        else _type = "string";
                        Variables.Add(new VariableNode(_name,_value, _type));
                        Debug.Log(_name+" "+_value+" "+ _type);
                    }
                    else if(str[0].StartsWith("InputNode")){
                        GameObject obj = GameObject.Find(str[0]);
                        string _name = obj.transform.GetChild(1).GetComponent<TMP_InputField>().text;
                        string _type;
                        string _value = InputValue;
                        if(float.TryParse(_value,out var number)) _type = "float";
                        else _type = "string";
                        Variables.Add(new VariableNode(_name,_value, _type));
                    }
                    else if(str[0].StartsWith("OutputNode")){
                        GameObject obj = GameObject.Find(str[0]);
                        OutputField.text = Variables.Find(x => x.Name == obj.transform.GetChild(1).GetComponent<TMP_InputField>().text).Value;
                        if(Variables.Find(x => x.Name == obj.transform.GetChild(1).GetComponent<TMP_InputField>().text).Value == Answer){
                            GameObject.Find("Correct").GetComponent<Animation>().Play();
                        }
                        else GameObject.Find("Incorrect").GetComponent<Animation>().Play();
                    }
                }
            }
        }
        string Assignment(int i, int str){
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