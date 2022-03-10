using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bBattery : MonoBehaviour
{
    // A custom variable type to store the number of Orbs that the battery has
    private enum NumberOfOrbs
    {
        One,
        Two,
        Three,
        Four
    };

    [SerializeField] private NumberOfOrbs numberOfOrbs; // An instance of the custom variable

    private Animator batteryAnim; // The battery's animator component

    private float animTime; // The point in the animation that should be currently playing
    
    [SerializeField] private GameObject TMProObj; // The text object that shows the battery's current power

    private TextMeshProUGUI mText; // The actual text that shows the battery's current power

    private GameObject battery; // UNUSED // Gets a reference to the player's battery object

    [SerializeField] private GameObject orbPrefab; // The prefab that stores the bad orbs object

    private GameObject tempClone; // Used to give info to a prefab that has just been spawned

    [SerializeField] private GameObject[] orbs; // An array to store all of the bad orbs

    // Only serialized for testing, REMOVE
    [SerializeField] private int connectedOrbs; // The number of orbs that are currently connected to devices

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
        // Check all of the orbs to see if they are connected to any of the devices by checking the distance between them and the Line Renderer point
        foreach (GameObject orb in orbs)
        {
            if (Vector3.Distance(orb.GetComponent<LineRenderer>().GetPosition(1), orb.transform.position) > 0.2)
            {
                connectedOrbs++;
            }
        }
        
        // If any of the orbs are connected change the bVar variable
        if (connectedOrbs > 0)
        {
            bVar.connectedToAnything = true;
        }
        else
        {
            bVar.connectedToAnything = false;
        }

        // Reset the no. of connected orbs
        connectedOrbs = 0;
    }

    // Checks how many orbs the battery has and spawns the prefabs in the correct positions
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

    // Finds all of the bad orb objects in the scene
    private void GetOrbs()
    {
        orbs = GameObject.FindGameObjectsWithTag("BadOrb");
    }

    // A continuously looping coroutine that increases the battery's power if it isn't connected to any devices
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
