using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class DialogSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private GameObject dialogField;
    [SerializeField] private float speed = 0.06f;
    [SerializeField] private string[] TextStrings = new string[9];
    private bool canPrint = true;
    private char[] b;
    private int i=0;
    private bool getE = false;
    private Coroutine printCoroutine;
    private Input _playerInput;
    public void OnEnable() {
        _playerInput = new Input();
        _playerInput.Enable();
        _playerInput.Player.Iteract.performed += PerformIteract;

    }

    public void OnDisable() {
        _playerInput.Player.Iteract.performed -= PerformIteract;
        _playerInput.Disable();

    }
    
    private void PerformIteract(InputAction.CallbackContext context)
    {
        if( i>=TextStrings.Length)
            CloseDialog();
        else
            getE = true;
    }
   


/////////////////////////////////////////////////////
    private void OnTriggerEnter2D(Collider2D other) {
        if(getE && other.gameObject.tag=="Player")
            getE = false;

    }
/////////////////////////////////////////////////////
    private void OnTriggerStay2D(Collider2D other) {
        if(getE && other.gameObject.tag=="Player" && canPrint){
            PreparePrint();
        }
        else if(getE && other.gameObject.tag=="Player" && !canPrint){
            speed = 0.01f;
            getE=false;
        }

    }
    
/////////////////////////////////////////////////////
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag=="Player"){
            CloseDialog();
        }

    }
/////////////////////////////////////////////////////
    public void OnClick(){
        if(i>=TextStrings.Length)
            CloseDialog();
        else if(canPrint){
            PreparePrint();
        }
        else if(!canPrint){
            speed = 0.01f;
        }
    }
/////////////////////////////////////////////////////
    private void PreparePrint(){
        b = TextStrings[i].ToCharArray();
        foreach (char x in b)
            b.ToString();

        txt.text = "";
        speed = 0.06f;
        dialogField.SetActive(true);
        canPrint = false;
        printCoroutine = StartCoroutine(Print());
        getE=false;
    }
/////////////////////////////////////////////////////
    IEnumerator Print(){
        for (int n = 0; n < TextStrings[i].Length; n++)
        {
            yield return new WaitForSeconds(speed);
            txt.text += b[n];    
        }
        i++;
        canPrint = true;
        getE=false;
    }
/////////////////////////////////////////////////////
    private void CloseDialog(){
        dialogField.SetActive(false);
        txt.text = "";
        getE=false;
        canPrint=true;
        i=0;
        if(printCoroutine!=null)
            StopCoroutine(printCoroutine);
    }

}
