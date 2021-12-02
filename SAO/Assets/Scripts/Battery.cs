using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public Animator batteryAnim;
    public float animTime;

    // Start is called before the first frame update
    void Start()
    {
        batteryAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the Battery Animation
        animTime = 1 - (gVar.batteryPercentage / 100);
        batteryAnim.Play("BatteryAction", 0, animTime);
        
    }
}
