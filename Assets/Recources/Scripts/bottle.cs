using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle : MonoBehaviour
{
    // Start is called before the first frame update
    public bool follow = false;
    public Transform hand;
    [SerializeField]
    private SphereCollider _trigger;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 rotationOffset;
    private Rigidbody rt;
    void Start()
    {
        rt = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand != null)
        {
            transform.SetParent(hand);
            transform.localPosition = new Vector3(-0.24f, -0.125f, -0.9f);
            transform.localRotation = Quaternion.Euler(51.6f, 89.27f, 85.6f);
            _trigger.enabled = false;
            rt.isKinematic = true;
        }
        else
        {
            _trigger.enabled = true;
            transform.parent = null;
            rt.isKinematic = false;
        }
    }
}
