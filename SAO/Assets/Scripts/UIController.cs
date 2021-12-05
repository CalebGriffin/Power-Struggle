using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image timerBackground;
    private float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        // 60 could easily be replaced with a variable for max time allowed in the level in seconds
        timerBackground.fillAmount = (60-timePassed) / 60; 
    }
}
