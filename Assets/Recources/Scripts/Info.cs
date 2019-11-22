using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Info : MonoBehaviour
{

    public GameLogic _info;
    public int index;
    [SerializeField]
    private TextMeshProUGUI text;

    private Animator _anim;
    private int lastLife;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        lastLife = _info._players[index].life;

    }

    // Update is called once per frame
    void Update()
    {

        if (lastLife != _info._players[index].life)
        {
            _anim.SetTrigger("flash");
            lastLife =  _info._players[index].life;
        }

        text.text = _info._players[index].name + ": " + _info._players[index].life;
        if (_info._players[index].life == 0) Destroy(gameObject, 1);
    }
}
