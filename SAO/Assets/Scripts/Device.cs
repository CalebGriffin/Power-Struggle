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
    [SerializeField] private int badConnectedNum;

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

    void GetCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, orbLayer);
        connectedNum = hitColliders.Length;

        Collider[] hitBadColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, badOrbLayer);
        badConnectedNum = hitBadColliders.Length;
    }
}
