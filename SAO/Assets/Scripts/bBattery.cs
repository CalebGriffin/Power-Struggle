using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bBattery : MonoBehaviour
{
    private enum NumberOfOrbs
    {
        One,
        Two,
        Three,
        Four
    };

    [SerializeField] private NumberOfOrbs numberOfOrbs;

    private Animator batteryAnim;

    private float animTime;
    
    [SerializeField] private GameObject TMProObj;

    private TextMeshProUGUI mText;

    private GameObject battery;

    [SerializeField] private GameObject orbPrefab;

    private GameObject tempClone;

    [SerializeField] private GameObject[] orbs;

    // Only serialized for testing, REMOVE
    [SerializeField] private int connectedOrbs;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the Components
        batteryAnim = GetComponent<Animator>();
        mText = TMProObj.GetComponent<TextMeshProUGUI>();

        // Spawn the correct number or orbs
        OrbSpawner();

        // Gets the orbs in the scene for reference
        GetOrbs();

        // Starts the coroutine which increases the battery
        StartCoroutine(BatteryUp());
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the Battery Animation
        animTime = 1f - (bVar.batteryPercentage / 100f);
        batteryAnim.Play("BatteryAction", 0, animTime);

        // Change the battery text
        mText.text = bVar.batteryPercentage.ToString();
    }

    void FixedUpdate()
    {
        foreach (GameObject orb in orbs)
        {
            if (Vector3.Distance(orb.GetComponent<LineRenderer>().GetPosition(1), orb.transform.position) > 0.2)
            {
                connectedOrbs++;
            }
        }
        
        if (connectedOrbs > 0)
        {
            bVar.connectedToAnything = true;
        }
        else
        {
            bVar.connectedToAnything = false;
        }

        connectedOrbs = 0;
    }

    private void OrbSpawner()
    {
        switch(numberOfOrbs)
        {
            case NumberOfOrbs.One:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0f); 
                break;
            
            case NumberOfOrbs.Two:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0.6f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -0.6f); 
                break;
            
            case NumberOfOrbs.Three:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 1.1f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -1.1f); 
                break;

            case NumberOfOrbs.Four:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 1.8f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0.6f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -0.6f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -1.8f); 
                break;

            default:
                break;
        }
    }

    private void GetOrbs()
    {
        orbs = GameObject.FindGameObjectsWithTag("BadOrb");
    }

    private IEnumerator BatteryUp()
    {
        yield return new WaitForSeconds(bVar.batteryUpWaitTime);

        if (bVar.connectedToAnything == false && bVar.batteryPercentage < 100)
        {
            bVar.batteryPercentage++;
        }

        StartCoroutine(BatteryUp());
    }
}
