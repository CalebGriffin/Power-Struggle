using System.Collections;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    // Only Serialized for testing // REMOVE
    [SerializeField] private GameObject[] orbs; // An array of GameObjects that store all of the orbs that the AI has
    [SerializeField] private GameObject[] devices; // An array of GameObjects that store all of the devices in the level

    [SerializeField] private GameObject aiBattery; // The aiBattery object for the AI

    // Only Serialized for testing // REMOVE
    [SerializeField] private float decisionTime; // Time waited before each AI decision
    [SerializeField] private int attackThreshold; // Used to decide if the AI should attack
    [SerializeField] private int doubleAttackThreshold; // Used to decide if the AI should attack 
    [SerializeField] private int defenceThreshold; // Used to decide if the AI should defend
    [SerializeField] private int doubleDefenceThreshold; // Used to decide if the AI should defend

    [SerializeField] private DifficultyLevel difficultyLevel; // The difficulty level can be set in the inspector for each level

    private int orbsAssigned; // Used to keep track of how many orbs have been used each decision
    private int totalOutput; // How much output will the AI make on the next turn

    private IDictionary<int, GameObject> orbsToConnect = new Dictionary<int, GameObject>(); // Gets reference to each orb and which device it should be connected to

    // Start is called before the first frame update
    void Start()
    {
        // Sets the difficulty of the AI
        SetDifficulty();
    }

    void Awake()
    {
        // Gets the Orbs and the Devices in the scene and adds them to their arrays
        StartCoroutine(GetDevicesAndOrbs());

        // Starts the coroutine which controls the decisions the AI makes
        StartCoroutine(Decision());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is run every time the AI makes a 'decision'
    private IEnumerator Decision()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(decisionTime);

        // Disconnect all of the orbs before deciding what to do
        Recharge();

        // Sorts the device array from smallest to largest based on it's current value
        devices = devices.OrderBy(go => go.GetComponent<Device>().currentVal).ToArray();

        // Resets the total power in the scene
        int totalPower = 0;

        // Calculates whether the player or the AI is winning based on current device values
        foreach(GameObject device in devices)
        {
            totalPower += device.GetComponent<Device>().currentVal;
        }

        // If the AI is winning or losing but not by a lot then 'attack'
        if (totalPower <= defenceThreshold && totalPower >= doubleAttackThreshold)
        {
            Attack(false);
        }
        // If the AI is losing by a lot then 'defend'
        else if (totalPower > defenceThreshold)
        {
            Defend(false);
        }
        // If the AI is winning by a lot then 'attack' and 'defend'
        else
        {
            AttackNDefend(false);
        }

        // Call the 'decision' coroutine again
        StartCoroutine(Decision());
    }

    // This function will target the devices that have the lowest power first in order to force the player to react
    // It takes a parameter of whether it has already checked the total output that the AI will make
    private void Attack(bool checkedOutput)
    {
        // Debug.Logs for testing
        Debug.Log("AI Decided to Attack!");
        Debug.Log("Checked Output is " + checkedOutput.ToString());

        // Resets the no. of orbs assigned and the total output and clears the dictionary
        orbsAssigned = 0;
        totalOutput = 0;
        orbsToConnect.Clear();
        
        foreach(GameObject device in devices)
        {
            // If all of the orbs have been assigned then end the loop
            if (orbs.Length - orbsAssigned == 0)
            {
                break;
            }

            // If the current device has a power above a certain limit then assign 2 orbs to be connected to it
            if (device.GetComponent<Device>().currentVal > doubleAttackThreshold && orbs.Length - orbsAssigned > 1)
            {
                // Increase the no. of orbs assigned
                orbsAssigned += 2;

                // If the AI's total output has already been checked, assign the orbs
                if (checkedOutput == true)
                {
                    // Debug.Log for testing
                    Debug.Log("Getting Here");

                    // Add the orbs to a dictionary which will be referenced later
                    orbsToConnect.Add((orbsAssigned - 2), device);

                    orbsToConnect.Add((orbsAssigned - 1), device);
                }
                // If the AI's total output hasn't already been checked, calculate how much power would be taken on the next go
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate * 2;
                }
            }
            // If the device's power is not above a certain limit then only assign 1 orb to connect to it
            else
            {
                orbsAssigned += 1;

                // If the AI's total output has already been checked, assign the orb
                if (checkedOutput == true)
                {
                    // Debug.Log for testing
                    Debug.Log("Getting Here");

                    // Add the orb to a dictionary which will be referenced later
                    orbsToConnect.Add((orbsAssigned - 1), device);
                }
                // If the AI's total output hasn't already been checked, calculate how much power would be taken on the next go
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate;
                }
            }
        }
        
        // If the AI's battery does have enough power to make the chosen move, call the recharge function
        if (totalOutput > bVar.batteryPercentage)
        {
            Recharge();
        }
        // If the AI has checked the output, call the function again with the parameter as true to assign the orbs
        else if (checkedOutput == false)
        {
            Attack(true);
        }

        // If the function has already assigned the orbs then call the coroutine to connect the orbs
        if (checkedOutput == true)
        {
            StartCoroutine(Connect());
        }
    }

    // This function will target the devices that have the highest power first in order to try and defend against the player's attacks
    // It takes a parameter of whether it has already checked the total output that the AI will make
    private void Defend(bool checkedOutput)
    {
        // Debug.Logs for testing
        Debug.Log("AI Decided to Defend!");
        Debug.Log("Checked Output is " + checkedOutput.ToString());

        // Resets the no. of orbs assigned and the total output and clears the dictionary
        orbsAssigned = 0;
        totalOutput = 0;
        orbsToConnect.Clear();

        // Reverses the device array to get the devices with the highest power
        System.Array.Reverse(devices);
        
        foreach(GameObject device in devices)
        {
            // If all of the robs have been assigned then end the loop
            if (orbs.Length - orbsAssigned == 0)
            {
                break;
            }

            // If the current device has a power above a certain limit then assign 2 orbs to be connected to it
            if (device.GetComponent<Device>().currentVal > doubleDefenceThreshold && orbs.Length - orbsAssigned > 1)
            {
                // Increase the no. of orbs assigned
                orbsAssigned += 2;

                // If the AI's total output has already been checked, assign the orbs
                if (checkedOutput == true)
                {
                    // Debug.Log for testing
                    Debug.Log("Getting Here");

                    // Add the orbs to a dictionary which will be referenced later
                    orbsToConnect.Add((orbsAssigned - 2), device);

                    orbsToConnect.Add((orbsAssigned - 1), device);
                }
                // If the AI's total output hasn't already been checked, calculate how much power would be taken on the next go
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate * 2;
                }
            }
            // If the device's power is not above a certain limit then only assign 1 orb to connect to it
            else
            {
                orbsAssigned += 1;

                // If the AI's total output has already been checked, assign the orb
                if (checkedOutput == true)
                {
                    // Debug.Log for testing
                    Debug.Log("Getting Here");

                    // Add the orb to a dictionary which will be referenced later
                    orbsToConnect.Add((orbsAssigned - 1), device);
                }
                // If the AI's total output hasn't already been checked, calculate how much power would be taken on the next go
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate;
                }
            }
        }
        
        // If the AI's battery does have enough power to make the chosen move, call the recharge function
        if (totalOutput > bVar.batteryPercentage)
        {
            Recharge();
        }
        // If the AI has checked the output, call the function again with the parameter as true to assign the orbs
        else if (checkedOutput == false)
        {
            Defend(true);
        }

        // If the function has already assigned the orbs then call the coroutine to connect the orbs
        if (checkedOutput)
        {
            StartCoroutine(Connect());
        }
    }

    // The AI needs to get the device array and use 2 pointer variables to alternate between connecting the device with the smallest value and the device with the greatest value
    // UNFINISHED
    private void AttackNDefend(bool checkedOutput)
    {
        Debug.Log("AI Decided to Attack and Defend!");
    }

    // Tells all of the orb objects to disconnect
    private void Recharge()
    {
        foreach (GameObject orb in orbs)
        {
            orb.SendMessage("Reset");
        }
    }

    // Coroutine to add a delay between connecting each orb to each device
    IEnumerator Connect()
    {
        // Repeat for each entry in the dictionary
        foreach(KeyValuePair<int, GameObject> kvp in orbsToConnect)
        {
            // Debug.Log for testing
            Debug.Log("Connecting Orb: " + kvp.Key + " to GameObject: " + kvp.Value.ToString());

            // Tell the orb in the dictionary entry to connect to the device in the dictionary entry
            orbs[kvp.Key].GetComponent<LineRenderer>().SetPosition(1, new Vector3(kvp.Value.transform.position.x, 0.1f, kvp.Value.transform.position.z));

            // Wait for the specified time before connecting the next orb
            yield return new WaitForSeconds(bVar.connectWaitTime);
        }
    }

    // This gives me 3 different fixed options for what the difficulty can be and shows as a dropdown menu in the inspector
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    };

    // Sets up all of the different variables based on the difficulty level of the AI
    private void SetDifficulty()
    {
        switch(difficultyLevel)
        {
            case DifficultyLevel.Easy:
                decisionTime = 5f;
                bVar.batteryUpWaitTime = 1f;
                bVar.connectWaitTime = 0.8f;

                attackThreshold = -10;
                doubleAttackThreshold = -20;
                defenceThreshold = 10;
                doubleDefenceThreshold = 20;
                break;

            case DifficultyLevel.Medium:
                decisionTime = 3f;
                bVar.batteryUpWaitTime = 1f;
                bVar.connectWaitTime = 0.8f;

                attackThreshold = -8;
                doubleAttackThreshold = -16;
                defenceThreshold = 8;
                doubleDefenceThreshold = 16;
                break;

            case DifficultyLevel.Hard:
                decisionTime = 2f;
                bVar.batteryUpWaitTime = 1f;
                bVar.connectWaitTime = 0.8f;

                attackThreshold = -6;
                doubleAttackThreshold = -12;
                defenceThreshold = 6;
                doubleDefenceThreshold = 12;
                break;

            default:
                break;
        }
    }

    // Gets all of the devices and orb objects in the scene and adds them to their respective arrays
    private IEnumerator GetDevicesAndOrbs()
    {
        yield return new WaitForSeconds(0.01f);

        orbs = GameObject.FindGameObjectsWithTag("BadOrb");

        devices = GameObject.FindGameObjectsWithTag("Device");
    }

    // Resets all of the necessary values back when the level is finished
    private void ResetLevel()
    {
        foreach (GameObject device in devices)
        {
            device.GetComponent<Device>().currentVal = 0;
        }
        gVar.batteryPercentage = 100;
        bVar.batteryPercentage = 100;
    }
}
