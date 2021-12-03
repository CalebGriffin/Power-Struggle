using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : MonoBehaviour
{
    private Animator batteryAnim;
    public float animTime;
    public GameObject TMProObj;
    private TextMeshProUGUI mText;

    // Start is called before the first frame update
    void Start()
    {
        batteryAnim = GetComponent<Animator>();
        mText = TMProObj.GetComponent<TextMeshProUGUI>();
        
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
}
