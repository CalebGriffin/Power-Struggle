using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Device : MonoBehaviour
{
    [SerializeField] private int maxVal; // The maximum power value for this device // UNUSED

    public int currentVal; // The current power value for this device

    public int powerRate; // The amount of power this device will take every second
    
    // Only serialized for testing, REMOVE
    [SerializeField] private int connectedNum; // The no. of connected Orbs
    [SerializeField] private int badConnectedNum; // The no. of connected bad Orbs

    [SerializeField] private Vector3 overlapBoxCenter; // The center of the Physics box

    [SerializeField] private LayerMask orbLayer; // The layer that all of the Orbs are on
    [SerializeField] private LayerMask badOrbLayer; // The layer that all of the bad Orbs are on

    [SerializeField] private TextMeshProUGUI mText; // The text above the device showing it's current power

    // The different colours for the text
    private Color whiteColour = Color.white;
    private Color greenColour = new Color(0.1058824f, 1f, 0.03529412f, 1f);
    private Color redColour = new Color(1f, 0.1960784f, 0.09019608f, 1f);

    private Animator deviceAnim; // The Animator component on the object

    // Start is called before the first frame update
    void Start()
    {
        // Set the center for the Physics box
        overlapBoxCenter = new Vector3(transform.position.x, 0f, transform.position.z);

        // Set up the Animator component
        AnimationSetup();

        // Starts the looping Coroutine
        StartCoroutine(PowerUp());
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Fixed Update is called 50 times per second
    void FixedUpdate()
    {
        // Get the currently connected Orbs
        GetCollisions();

        // Updated whether the animation should be playing
        AnimationUpdater();
    }

    // Get the currently connected Orbs
    void GetCollisions()
    {
        // Get an array of colliders that are within the Physics box and tally the numbers of bad and good Orbs
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenter, transform.localScale / 2, Quaternion.identity, orbLayer);
        connectedNum = hitColliders.Length;

        Collider[] hitBadColliders = Physics.OverlapBox(overlapBoxCenter, transform.localScale / 2, Quaternion.identity, badOrbLayer);
        badConnectedNum = hitBadColliders.Length;
        Debug.Log("Bad Connected Num: " + badConnectedNum.ToString());
    }

    // A continuously looping Coroutine that takes power from the batteries
    IEnumerator PowerUp()
    {
        yield return new WaitForSeconds(1f);

        // Take power from the player's battery
        if (gVar.batteryPercentage > powerRate * connectedNum)
        {
            currentVal += powerRate * connectedNum;
            gVar.batteryPercentage -= powerRate * connectedNum;
        }

        // Take power from the AI's battery
        if (bVar.batteryPercentage > powerRate * badConnectedNum)
        {
            currentVal -= powerRate * badConnectedNum;
            bVar.batteryPercentage -= powerRate * badConnectedNum;
        }

        // Update the text above the Device
        TextUpdater();

        // Call the coroutine again continuously
        StartCoroutine(PowerUp());
    }

    // Updates the text above the Device
    private void TextUpdater()
    {
        // Change the text to the Device's current power
        mText.text = currentVal.ToString();

        // Change the colour of the text based on the Device's current power
        if (currentVal > 0)
        {
            mText.color = greenColour;
        }
        else if (currentVal < 0)
        {
            mText.color = redColour;
        }
        else
        {
            mText.color = whiteColour;
        }
    }

    // If the Device is connected to any battery then it should be animating
    private void AnimationUpdater()
    {
        if (connectedNum > 0 || badConnectedNum > 0)
        {
            deviceAnim.speed = 1;
        }
        else
        {
            deviceAnim.speed = 0;
        }
    }

    // Get a reference to the Animator Component on the object
    private void AnimationSetup()
    {
        deviceAnim = GetComponent<Animator>();

        // UNUSED // Was to set up the names of the custom device animations
        //string deviceName = this.gameObject.ToString();
        //switch (deviceName)
        //{
            //case "Camera":
                //animationName = "GrowNShrink";
                //break;

            //case "Blender":
                //animationName = "Blend";
                //break;

            //default:
                //break;
        //}
    }
}
