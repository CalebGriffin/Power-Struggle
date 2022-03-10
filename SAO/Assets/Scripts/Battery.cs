using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : MonoBehaviour
{
    // A special variable to allow selecting the number or orbs that the battery has in the inspector
    public enum NumberOfOrbs
    {
        One,
        Two,
        Three,
        Four
    };

    public NumberOfOrbs numberOfOrbs; // Stores the number of orbs that the battery has

    private Animator batteryAnim; // Gets reference to the Animator attached to this object

    private float animTime; // A float variable to control where in the animation the Animator should play
    
    [SerializeField] private GameObject TMProObj; // The text object that shows the battery's current power

    private TextMeshProUGUI mText; // The actual text that shows the battery's current power

    [SerializeField] private GameObject rechargeCanvas; // The canvas that stores the recharge button

    [SerializeField] private GameObject orbPrefab; // The prefab that stores the orb object

    private GameObject tempClone; // Used to give info to a prefab that has just been spawned

    private GameObject[] orbs; // An array to store all of the orbs

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
        animTime = 1f - (gVar.batteryPercentage / 100f);
        batteryAnim.Play("BatteryAction", 0, animTime);

        // Change the battery text
        mText.text = gVar.batteryPercentage.ToString();
    }

    void FixedUpdate()
    {
        foreach (GameObject orb in orbs)
        {
            // Check all of the orbs to see if they are connected to any of the devices by checking the distance between them and the Line Renderer point
            if (Vector3.Distance(orb.GetComponent<LineRenderer>().GetPosition(1), orb.transform.position) > 0.2)
            {
                connectedOrbs++;
            }
        }
        
        // If any of the orbs are connected, change the gVar variable
        if (connectedOrbs > 0)
        {
            gVar.connectedToAnything = true;
            //rechargeCanvas.SetActive(true);
        }
        else
        {
            gVar.connectedToAnything = false;
            //rechargeCanvas.SetActive(false);
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

    // Finds all of the orb objects in the scene
    private void GetOrbs()
    {
        orbs = GameObject.FindGameObjectsWithTag("Orb");
    }

    // A continuously looping coroutine that increases the battery's power if it isn't connected to any devices
    private IEnumerator BatteryUp()
    {
        yield return new WaitForSeconds(0.5f);

        if (gVar.connectedToAnything == false && gVar.batteryPercentage < 100)
        {
            gVar.batteryPercentage++;
        }

        StartCoroutine(BatteryUp());
    }

    // Disconnects all of the orbs from their devices and resets the Line Renderer positions by calling a function on each orb
    public void ResetOrbs()
    {
        foreach (GameObject orb in orbs)
        {
            orb.SendMessage("Reset");
        }
    }
}
