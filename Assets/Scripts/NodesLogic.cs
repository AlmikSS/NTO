using System.Collections.Generic;
using UnityEngine;

public class NodesLogic : MonoBehaviour
{
    [HideInInspector] public List<string> Programm = new List<string>();
    public void OnDrop(GameObject Obj){
        BlocksMovement _blocksMove = GameObject.Find("Canvas").GetComponent<BlocksMovement>(); 

        string _edge =  _blocksMove.DropBlock(Obj);
        GameObject _coll = _blocksMove.Coll;
        if(Obj.name == "BeginNode" && Programm.Count == 0){
            Programm.Add(Obj.name);
            Programm.Add(_coll.name);
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