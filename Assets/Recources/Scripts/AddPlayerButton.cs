using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddPlayerButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tmp;
    public Transform parent;
    public TMP_InputField nameInput;
    public TMP_InputField ticketInput;
    public TextMeshProUGUI _text;

    public GameLogic config;
    public void AddPlayerClicked()
    {
        if (nameInput.text != "" && ticketInput.text != "")
        {
            _text.text = "" + nameInput.text + " , " + ticketInput.text;
            config.AddNewPlayer(nameInput.text, ticketInput.text);
            nameInput.text = "";
            ticketInput.text = "";
            GameObject name_tmp = Instantiate(tmp, Vector3.zero, Quaternion.identity);
            name_tmp.transform.SetParent(parent.transform);
            
        }
    }
}
