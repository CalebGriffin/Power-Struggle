using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour
{
    [SerializeField] private int maxVal;
    [SerializeField] private int currentVal;
    [SerializeField] private int powerRate;
    // Only serialized for testing, REMOVE
    [SerializeField] private int connectedNum;
    [SerializeField] private LayerMask orbLayer;
    [SerializeField] private LayerMask badOrbLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        GetCollisions();
    }

    // FAILED ATTEMPT
    /*
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Orb"))
        {
            connectedNum++;
            gVar.numOfConnections++;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        Debug.Log("OnTriggerExit Called");
        if (col.CompareTag("Orb"))
        {
            connectedNum--;
            gVar.numOfConnections--;
        }
    }

    public void Connected()
    {
        Debug.Log("Getting Here");
        connectedNum++;
        gVar.numOfConnections++;
    }

    public void Disconnected()
    {
        Debug.Log("Getting Here2");
        connectedNum--;
        gVar.numOfConnections--;
    }
    */

    void GetCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, orbLayer);
        connectedNum = hitColliders.Length;

        //Collider[] hitBadColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, orbLayer);
        //connectedNum -= hitBadColliders.Length;
    }
}
