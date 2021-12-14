using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private GameObject[] devices;
    [SerializeField] private TextMeshProUGUI resultText;
    private Color whiteColour = Color.white;
    private Color greenColour = new Color(0.1058824f, 1f, 0.03529412f, 1f);
    private Color redColour = new Color(1f, 0.1960784f, 0.09019608f, 1f);

    [SerializeField] private Image timerBackground;

    private float timePassed;

    [SerializeField] private GameObject pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        devices = GameObject.FindGameObjectsWithTag("Device"); 
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        // 60 could easily be replaced with a variable for max time allowed in the level in seconds
        timerBackground.fillAmount = (60-timePassed) / 60; 
    }

    void FixedUpdate()
    {
        int totalPower = 0;
        foreach (GameObject device in devices)
        {
            totalPower += device.GetComponent<Device>().currentVal;
        }

        if (totalPower > 0)
        {
            resultText.text = "WINNING";
            resultText.color = greenColour; 
        }
        else if (totalPower < 0)
        {
            resultText.text = "LOSING";
            resultText.color = redColour;
        }
        else
        {
            resultText.text = "DRAWING";
            resultText.color = whiteColour;
        }
        

    }

    public void PauseButton()
    {
        if (pauseCanvas.activeSelf == false)
        {
            Time.timeScale = 0;

            pauseCanvas.SetActive(true);
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;

        pauseCanvas.SetActive(false);
    }
}
