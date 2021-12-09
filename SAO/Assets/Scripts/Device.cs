using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Device : MonoBehaviour
{
    [SerializeField] private int maxVal;

    public int currentVal;

    public int powerRate;
    
    // Only serialized for testing, REMOVE
    [SerializeField] private int connectedNum;
    [SerializeField] private int badConnectedNum;

    [SerializeField] private LayerMask orbLayer;
    [SerializeField] private LayerMask badOrbLayer;

    [SerializeField] private TextMeshProUGUI mText; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PowerUp());
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

    IEnumerator PowerUp()
    {
        yield return new WaitForSeconds(1f);

        if (gVar.batteryPercentage > powerRate * connectedNum)
        {
            currentVal += powerRate * connectedNum;
            gVar.batteryPercentage -= powerRate * connectedNum;
        }

        
        if (bVar.batteryPercentage > powerRate * badConnectedNum)
        {
            currentVal -= powerRate * badConnectedNum;
            bVar.batteryPercentage -= powerRate * badConnectedNum;
        }

        mText.text = currentVal.ToString();

        StartCoroutine(PowerUp());
    }
}
