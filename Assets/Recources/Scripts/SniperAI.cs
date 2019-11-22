using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SniperAI : MonoBehaviour
{

    public Transform npc;
    public Camera _camera;
    public Transform _scopeCam;

    private RectTransform rt;

    [SerializeField]
    private GameLogic _gamelogic;

    public Vector3 m_Offset = Vector3.zero;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    public float smooth = 0.01f;
    [SerializeField]
    private GameObject crown;

    private float speed;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _camera = Camera.main;
        npc = _gamelogic._npcToFollow;
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        npc = _gamelogic._npcToFollow;
        // Debug.Log(npc.position);
        if (npc != null)
        {
            Vector3 pos = npc.GetComponent<LookAt>().head.transform.position;
            Vector3 tempPos = _camera.WorldToScreenPoint(pos);
            float dist = Vector3.Distance(tempPos, rt.position);
            rt.position = Vector3.SmoothDamp(rt.position, _camera.WorldToScreenPoint(pos), ref velocity, speed);
            //Vector3 camerapos =Vector3.SmoothDamp(rt.position, _camera.WorldToScreenPoint(pos), ref velocity, speed);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(rt.position);
            if (worldPosition.y < 0) worldPosition.y = 0f;
            _scopeCam.position = worldPosition;
            //Debug.Log(dist);
            if (dist < 20 && dist > 5)
            {
                _name.text = "" + npc.GetComponent<npcAI>().name;
                speed = 0.05f;
            }
            else if (dist < 5)
            {
                _name.text = "" + npc.GetComponent<npcAI>().name;
                speed = 0f;
            }
            else
            {
                _name.text = "";
                speed = 0.1f;

            }

        }

    }

    public void Fire()
    {
        _animator.SetTrigger("Fire");
    }

    public void newTarget()
    {
        _animator.SetTrigger("ShowScope");
        speed = 0.1f;
    }

    public void showCrown()
    {
        crown.SetActive(true);
    }

    public void showScope()
    {
        _animator.SetTrigger("show");
    }

}
