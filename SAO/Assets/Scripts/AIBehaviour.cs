using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    // Only Serialized for testing // REMOVE
    [SerializeField] private GameObject[] orbs; // An array of GameObjects that store all of the orbs that the AI has
    [SerializeField] private GameObject[] devices; // An array of GameObjects that store all of the devices in the level

    // Only Serialized for testing // REMOVE
    [SerializeField] private float decisionTime; // Time waited before each AI decision
    [SerializeField] private int attackThreshold; // Used to decide if the AI should attack
    [SerializeField] private int doubleAttackThreshold; // Used to decide if the AI should attack 
    [SerializeField] private int defenseThreshold; // Used to decide if the AI should defend
    [SerializeField] private int doubleDefenseThreshold; // Used to decide if the AI should defend

    [SerializeField] private DifficultyLevel difficultyLevel; // The difficulty level can be set in the inspector for each level

    private int orbsAssigned; // Used to keep track of how many orbs have been used each decision
    private int totalOutput; // How much output will the AI make on the next turn

    // Start is called before the first frame update
    void Start()
    {
        // Runs the function to set the difficulty of the AI
        SetDifficulty();

        // Gets the Orbs and the Devices in the scene and adds them to their arrays
        GetDevicesAndOrbs();

        // Starts the coroutine which controls the decisions the AI makes
        StartCoroutine(Decision());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Decision()
    {
        yield return new WaitForSeconds(decisionTime);

        Recharge();

        devices = devices.OrderBy(go => go.GetComponent<Device>().currentVal).ToArray();

        int totalPower = 0;
        // Just used for testing // REMOVE
        int i = 0;
        foreach(GameObject device in devices)
        {
            // Just used for testing // REMOVE
            //Debug.Log(i + ": " + devices[i].ToString());
            i++;

            totalPower += device.GetComponent<Device>().currentVal;
        }

        if (totalPower <= defenseThreshold && totalPower >= doubleAttackThreshold)
        {
            Attack(false);
        }
        else if (totalPower > defenseThreshold)
        {
            Defend(false);
        }
        else
        {
            AttackNDefend(false);
        }

        StartCoroutine(Decision());
    }

    private void Attack(bool checkedOutput)
    {
        Debug.Log("AI Decided to Attack!");

        orbsAssigned = 0;
        totalOutput = 0;
        
        foreach(GameObject device in devices)
        {
            if (orbs.Length - orbsAssigned == 0)
            {
                break;
            }

            if (device.GetComponent<Device>().currentVal > doubleAttackThreshold && orbs.Length - orbsAssigned > 1)
            {
                orbsAssigned += 2;

                if (checkedOutput == true)
                {
                    orbs[orbsAssigned - 2].GetComponent<LineRenderer>().SetPosition(1, new Vector3(device.transform.position.x, 0.1f, device.transform.position.z));
                    orbs[orbsAssigned - 1].GetComponent<LineRenderer>().SetPosition(1, new Vector3(device.transform.position.x, 0.1f, device.transform.position.z));
                }
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate * 2;
                }
            }
            else
            {
                orbsAssigned += 1;

                if (checkedOutput == true)
                {
                    orbs[orbsAssigned - 1].GetComponent<LineRenderer>().SetPosition(1, new Vector3(device.transform.position.x, 0.1f, device.transform.position.z));

                }
                else
                {
                    totalOutput += device.GetComponent<Device>().powerRate;
                }
            }
        }
        
        if (totalOutput > bVar.batteryPercentage)
        {
            Recharge();
        }
        else if (checkedOutput == false)
        {
            Attack(true);
        }
    }

    private void Defend(bool checkedOutput)
    {
        Debug.Log("AI Decided to Defend!");
    }

    private void AttackNDefend(bool checkedOutput)
    {
        Debug.Log("AI Decided to Attack and Defend!");
    }

    private void Recharge()
    {
        foreach (GameObject orb in orbs)
        {
            orb.SendMessage("Reset");
        }
    }

    // This gives me 3 different fixed options for what the difficulty can be and shows as a dropdown menu in the inspector
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    };

    private void SetDifficulty()
    {
        switch(difficultyLevel)
        {
            case DifficultyLevel.Easy:
                decisionTime = 5f;
                attackThreshold = -10;
                doubleAttackThreshold = -20;
                defenseThreshold = 10;
                doubleDefenseThreshold = 20;
                break;

            case DifficultyLevel.Medium:
                decisionTime = 3f;
                attackThreshold = -8;
                doubleAttackThreshold = -16;
                defenseThreshold = 8;
                doubleDefenseThreshold = 16;
                break;

            case DifficultyLevel.Hard:
                decisionTime = 2f;
                attackThreshold = -6;
                doubleAttackThreshold = -12;
                defenseThreshold = 6;
                doubleDefenseThreshold = 12;
                break;
        }
    }

    private void GetDevicesAndOrbs()
    {
        orbs = GameObject.FindGameObjectsWithTag("BadOrb");

        devices = GameObject.FindGameObjectsWithTag("Device");
    }
}
