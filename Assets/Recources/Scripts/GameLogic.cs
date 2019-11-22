using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    string path = "names.txt";
    [SerializeField]
    private GameObject screenParent;
    [SerializeField]
    private GameObject[] npcPrefab;

    [SerializeField]
    private List<GameObject> Ticketlist;
    [SerializeField]
    private SniperAI scope;
    [SerializeField]
    private float raffleTime = 25f;
    [SerializeField]
    private float glowTime = 0.5f;
    [SerializeField]
    private float followTime = 10f;


    private bool _select = false;
    private float timeLeft;
    private int lastNPC;
    private int changesBeforeKill = 0;
    private int count;
    private bool kill = false;
    [SerializeField]
    private int minChanges = 1;
    [SerializeField]
    private int maxChanges = 2;
    [SerializeField]
    private AudioSource bulletSound;

    [SerializeField]
    private GameObject infoObj;

    public GameObject bottle;


    [SerializeField]
    private Transform npcToFollow
    {
        get { return _npcToFollow; }
        set { _npcToFollow = value; }
    }
    public Transform _npcToFollow;

    [System.Serializable]
    public class Players
    {
        public string name = "";
        public int life = 0;
        public Players(string name, int life)
        {
            this.name = name;
            this.life = life;
        }

    }

    public List<Players> _players;
    private bool start = false;
    private int playerlines = 0;


    void Start()
    {
        Application.targetFrameRate = 60;
        timeLeft = raffleTime;
        _players = new List<Players>();

        //ReadString();

    }

    /*     void ReadString()
        {
            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);
            StreamReader tReader = new StreamReader(path);
            string line;
            // Read and display lines from the file until the end of 
            // the file is reached.
            int colornum = 0;
            int lines = 0;

            string temp = tReader.ReadLine() ?? "";
            string[] config = temp.Split(',');
            Debug.Log("fristline " + temp);
           // _players = new Players[int.Parse(config[1])];
            minChanges = int.Parse(config[2]);
            maxChanges = int.Parse(config[3]);
            followTime = int.Parse(config[4]);
            while ((line = reader.ReadLine()) != null)
            {
                if (!line.Contains("CONFIG"))
                {
                    string[] codes = line.Split(',');
                    string _name = codes[0];
                    int _life = int.Parse(codes[1]);
                    _players[lines] = new Players(_name, _life);
                    // _players[lines].life = _life;
                    lines++;
                }
            }


            for (int i = 0; i < _players.Length; i++)
            {
                GameObject info = Instantiate(infoObj, Vector3.zero, Quaternion.identity);

                info.transform.SetParent(screenParent.transform);
                info.transform.localScale = Vector3.one;
                info.GetComponent<Info>()._info = this;
                info.GetComponent<Info>().index = i;
                for (int j = 0; j < _players[i].life; j++)
                {
                    colornum++;
                    if (colornum == 6) colornum = 0;
                    float x = Random.Range(0, 20);
                    float y = Random.Range(-25, 25);
                    GameObject obj = Instantiate(npcPrefab[colornum], new Vector3(x, 1, y), Quaternion.identity);
                    Debug.Log(_players[i].name);
                    obj.GetComponent<npcAI>().name = _players[i].name;
                    Ticketlist.Add(obj);
                }
            }


            reader.Close();
            for (int i = 0; i < 10; i++)
                Utils.Shuffle(Ticketlist);

        } */

    public void AddNewPlayer(string name, string life)
    {
        int _life = int.Parse(life);
        //
        _players.Add(new Players(name, _life));
    }

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

/*         if (start)
        {
            //start = true;
            int player = Random.Range(0, Ticketlist.Count);
            lastNPC = player;
            _npcToFollow = Ticketlist[player].transform;
            scope.enabled = true;
            scope.showScope();
            changesBeforeKill = Random.Range(minChanges, maxChanges);
            StartCoroutine(changeNPC());
        } */

        if (bottle.GetComponent<bottle>().hand == null && Ticketlist.Count > 1 && start)
        {
            int player = Random.Range(0, Ticketlist.Count);
            Ticketlist[player].GetComponent<npcAI>().GetToBottle(bottle.transform);
        }
    }

    public void startTheGame()
    {
        int colornum = 0;
        for (int i = 0; i < _players.Count; i++)
        {
            GameObject info = Instantiate(infoObj, Vector3.zero, Quaternion.identity);

            info.transform.SetParent(screenParent.transform);
            info.transform.localScale = Vector3.one;
            info.GetComponent<Info>()._info = this;
            info.GetComponent<Info>().index = i;
            for (int j = 0; j < _players[i].life; j++)
            {
                colornum++;
                if (colornum == 6) colornum = 0;
                float x = Random.Range(0, 20);
                float y = Random.Range(-25, 25);
                GameObject obj = Instantiate(npcPrefab[colornum], new Vector3(x, 1, y), Quaternion.identity);
                Debug.Log(_players[i].name);
                obj.GetComponent<npcAI>().name = _players[i].name;
                Ticketlist.Add(obj);
            }
        }

        for (int i = 0; i < 10; i++)
            Utils.Shuffle(Ticketlist);

        StartCoroutine(startGame());
        //start = true;
    }


    IEnumerator startGame()
    {
        yield return new WaitForSecondsRealtime(10);
        start = true;
        int player = Random.Range(0, Ticketlist.Count);
        lastNPC = player;
        _npcToFollow = Ticketlist[player].transform;
        scope.enabled = true;
        scope.showScope();
        changesBeforeKill = Random.Range(minChanges, maxChanges);
        StartCoroutine(changeNPC());
    }

    IEnumerator changeNPC()
    {
        yield return new WaitForSecondsRealtime(followTime);
        Debug.Log("changes " + count + " changesBeforeKill " + changesBeforeKill);
        count++;
        if (count >= changesBeforeKill)
        {

            if (Ticketlist.Count > 1)
            {
                scope.smooth = 1f;
                scope.Fire();
                bulletSound.Play();
                _npcToFollow.GetComponent<npcAI>().Die();
                string value = _npcToFollow.GetComponent<npcAI>().name;
                for (int i = 0; i < _players.Count; i++)
                {
                    if (_players[i].name == value) _players[i].life--;
                }

                _npcToFollow = null;
                Ticketlist.RemoveAt(lastNPC);
                StartCoroutine(wait());
            }


        }
        else
        {
            if (Ticketlist.Count > 0)
            {
                int player = Random.Range(0, Ticketlist.Count);
                lastNPC = player;
                _npcToFollow = Ticketlist[player].transform;
                scope.newTarget();
                StartCoroutine(changeNPC());
            }

            if (Ticketlist.Count == 1)
            {
                scope.showCrown();
                _npcToFollow.GetComponent<npcAI>().Party();
            }
        }

    }

    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(followTime);
        int player = Random.Range(0, Ticketlist.Count);
        lastNPC = player;
        _npcToFollow = Ticketlist[player].transform;
        scope.newTarget();
        if (Ticketlist.Count > 0)
        {
            StartCoroutine(changeNPC());
            changesBeforeKill = Random.Range(minChanges, maxChanges);
            count = 0;
        }
    }


}