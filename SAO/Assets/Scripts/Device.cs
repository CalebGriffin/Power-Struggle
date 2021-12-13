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
    private Color whiteColour = Color.white;
    private Color greenColour = new Color(0.1058824f, 1f, 0.03529412f, 1f);
    private Color redColour = new Color(1f, 0.1960784f, 0.09019608f, 1f);

    private Animator deviceAnim;
    private float animationTime;
    private string animationName;

    // Start is called before the first frame update
    void Start()
    {
        AnimationSetup();

        StartCoroutine(PowerUp());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        GetCollisions();

        AnimationUpdater();
    }

    void GetCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, orbLayer);
        connectedNum = hitColliders.Length;

        Collider[] hitBadColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, badOrbLayer);
        badConnectedNum = hitBadColliders.Length;
        Debug.Log("Bad Connected Num: " + badConnectedNum.ToString());
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

        TextUpdater();

        StartCoroutine(PowerUp());
    }

    private void TextUpdater()
    {
        mText.text = currentVal.ToString();

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

    private void AnimationUpdater()
    {
        if (connectedNum > 0 || badConnectedNum > 0)
        {
            animationTime += Time.deltaTime;
            deviceAnim.speed = 1;
            deviceAnim.Play(animationName, 0, animationTime);
        }
        else
        {
            animationTime = 0;
            deviceAnim.speed = 0;
            deviceAnim.Play(animationName, 0, 0);
        }
    }

    private void AnimationSetup()
    {
        deviceAnim = GetComponent<Animator>();
        string deviceName = this.gameObject.ToString();
        switch (deviceName)
        {
            case "Camera":
                animationName = "GrowNShrink";
                break;

            default:
                break;
        }
    }
}
