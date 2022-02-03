using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    private GameObject[] devices; // An array of GameObjects that store all of the devices in the level

    [SerializeField] private TextMeshProUGUI resultText; // The text that tells the player whether they are winning, losing or drawing

    [SerializeField] private GameObject numbersTextObj; // The GameObject that has the UI for the timer countdown
    [SerializeField] private TextMeshProUGUI numbersText; // The text that tells when they are running out of time

    // These are the different colours for the text
    private Color whiteColour = Color.white;
    private Color greenColour = new Color(0.1058824f, 1f, 0.03529412f, 1f);
    private Color redColour = new Color(1f, 0.1960784f, 0.09019608f, 1f);

    private bool countdownAnimationStarted = false;
    private int countdownTime = 10;

    [SerializeField] private Image timerBackground; // The background of the timer

    private float timePassed; // Float variable to store how long the game has been running

    [SerializeField] private GameObject pauseCanvas; // Canvas which will appear when the game is paused

    // Start is called before the first frame update
    void Start()
    {
        // Get all of the devices currently in the scene
        devices = GameObject.FindGameObjectsWithTag("Device"); 

        numbersText = numbersTextObj.GetComponent<TextMeshProUGUI>();
        numbersTextObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the amount of time that has passed by how long it has been since the last frame update
        timePassed += Time.deltaTime;

        // NOTE: 60 could easily be replaced with a variable for max time allowed in the level in seconds
        timerBackground.fillAmount = (60-timePassed) / 60; 

        if (60 - timePassed <= 10 && !countdownAnimationStarted)
        {
            countdownAnimationStarted = true;
            numbersTextObj.SetActive(true);
            StartCoroutine(CountdownAnim());
        }

        if (timePassed >= 60)
        {
            GameOver();
        }
    }

    private IEnumerator CountdownAnim()
    {
        numbersText.text = countdownTime.ToString();
        numbersTextObj.GetComponent<Animator>().Play("ScaleIn");
        yield return new WaitForSeconds(0.5f);
        numbersTextObj.GetComponent<Animator>().Play("ScaleOut");
        yield return new WaitForSeconds(0.5f);
        countdownTime--;
        if (countdownTime > 0)
        {
            StartCoroutine(CountdownAnim());
        }
    }

    private void GameOver()
    {

    }

    // FixedUpdate is called 50 times per second regardless of framerate
    void FixedUpdate()
    {
        // Calculate which player is winning based on the current values of all the devices
        int totalPower = 0;
        foreach (GameObject device in devices)
        {
            totalPower += device.GetComponent<Device>().currentVal;
        }

        // Change the UI text to display whether the player is winning, losing or drawing
        switch (totalPower)
        {
            case int x when x > 0:
                resultText.text = "WINNING";
                resultText.color = greenColour; 
                break;

            case int x when x < 0:
                resultText.text = "LOSING";
                resultText.color = redColour;
                break;

            default:
                resultText.text = "DRAWING";
                resultText.color = whiteColour;
                break;
        }
    }

    // PauseButton is called when the pause button is pressed
    public void PauseButton()
    {
        // If the game is not already paused then stop time and display the pause menu
        if (pauseCanvas.activeSelf == false)
        {
            Time.timeScale = 0;

            pauseCanvas.SetActive(true);
        }
    }

    // ResumeButton is called when the resume button is pressed
    public void ResumeButton()
    {
        // Set the time back to normal scale and hide the pause menu
        Time.timeScale = 1;

        pauseCanvas.SetActive(false);
    }
}
