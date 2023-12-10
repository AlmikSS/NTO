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
    private int i = 0;
    private bool getE = false, _first = true;
    private Coroutine printCoroutine;
    private Input _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
        _playerInput.Player.Iteract.performed += context => PerformIteract();
    }

    public void OnEnable() { _playerInput.Enable(); }

    public void OnDisable() { _playerInput.Disable(); }


    private void PerformIteract()
    {
        if (_first)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            getE = true;
            _first = false;
        }
    }



    /////////////////////////////////////////////////////
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (getE && other.gameObject.tag == "Player")
            getE = false;

    }
    /////////////////////////////////////////////////////
    private void OnTriggerStay2D(Collider2D other)
    {
        if (getE && other.gameObject.tag == "Player")
        {
            PreparePrint();
            getE = false;
        }

    }

    /////////////////////////////////////////////////////
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloseDialog();
        }

    }
    /////////////////////////////////////////////////////
    public void OnClick()
    {
        if (i >= TextStrings.Length && canPrint)
            CloseDialog();
        else if (canPrint)
        {
            PreparePrint();
        }
        else if (!canPrint)
        {
            speed = 0.01f;
        }
    }
    /////////////////////////////////////////////////////
    private void PreparePrint()
    {
        b = TextStrings[i].ToCharArray();
        foreach (char x in b)
            b.ToString();

        txt.text = "";
        speed = 0.06f;
        dialogField.SetActive(true);
        canPrint = false;
        printCoroutine = StartCoroutine(Print());
    }
    /////////////////////////////////////////////////////
    IEnumerator Print()
    {
        for (int n = 0; n < TextStrings[i].Length; n++)
        {
            yield return new WaitForSeconds(speed);
            txt.text += b[n];
        }
        i++;
        canPrint = true;
    }
    /////////////////////////////////////////////////////
    private void CloseDialog()
    {
        dialogField.SetActive(false);
        txt.text = "";
        getE = false;
        _first = true;
        canPrint = true;
        i = 0;
        if (printCoroutine != null)
            StopCoroutine(printCoroutine);
    }

}
