using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAI : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;

    public string name;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;
    private float _timer;
    private bool dead = false;
    [SerializeField]
    private Transform Lhand;

    [SerializeField]
    private GameObject bloodFx;

    private Transform _bottle;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _timer = Random.Range(1, wanderTimer);
        timer = wanderTimer;
        SetKinematic(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= _timer && !dead)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void GetToBottle(Transform bottle)
    {
        if (bottle)
        {
            _bottle = bottle;
            agent.destination = bottle.position;
        }

    }
    void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }


    public void Die()
    {
        dead = true;
        if (_bottle != null)
        {
            _bottle.parent = null;
            _bottle.GetComponent<bottle>().hand = null;
        }
        GameObject obj = Instantiate(bloodFx, GetComponent<LookAt>().head.position, Quaternion.identity);
        GetComponent<Animator>().enabled = false;
        GetComponent<LocomotionSimpleAgent>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        agent.enabled = false;
        SetKinematic(false);
        GetComponent<LookAt>().head.GetComponent<Rigidbody>().AddForce(0f, 100f, 0f);
        Destroy(gameObject, 15);
        Destroy(obj, 15);
    }

    public void Party()
    {
        GetComponent<Animator>().SetTrigger("Party");
        agent.enabled = false;
        GetComponent<LocomotionSimpleAgent>().enabled = false;
        dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER " + other.tag);
        other.GetComponent<bottle>().hand = Lhand;
    }

}
