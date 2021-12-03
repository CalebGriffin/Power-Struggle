using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : MonoBehaviour
{
    private Animator batteryAnim;

    private float animTime;
    
    [SerializeField] private GameObject TMProObj;

    private TextMeshProUGUI mText;

    [SerializeField] private int orbNum;

    [SerializeField] private GameObject orbPrefab;

    private GameObject tempClone;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the Components
        batteryAnim = GetComponent<Animator>();
        mText = TMProObj.GetComponent<TextMeshProUGUI>();

        // Spawn the correct number or orbs
        OrbSpawner();

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

    private void OrbSpawner()
    {
        switch(orbNum)
        {
            case 1:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0f); 
                break;
            
            case 2:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0.6f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -0.6f); 
                break;
            
            case 3:
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 0f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, 1.1f); 
                tempClone = Instantiate(orbPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
                tempClone.transform.localPosition = new Vector3(1.8f, 0f, -1.1f); 
                break;

            case 4:
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

    private IEnumerator BatteryUp()
    {
        yield return new WaitForSeconds(0.5f);

        if (gVar.connectedToAnything == false && gVar.batteryPercentage < 100)
        {
            gVar.batteryPercentage++;
        }

        StartCoroutine(BatteryUp());
    }
}
