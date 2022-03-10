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
    [SerializeField] private TextMeshProUGUI numbersText; // The text that appears when they are running out of time
    [SerializeField] private TextMeshProUGUI winnerText; // The text that tells the player who has won

    // These are the different colours for the text
    private Color whiteColour = Color.white;
    private Color greenColour = new Color(0.1058824f, 1f, 0.03529412f, 1f);
    private Color redColour = new Color(1f, 0.1960784f, 0.09019608f, 1f);

    private bool countdownAnimationStarted = false;
    private int countdownTime = 10;
    [SerializeField] private int maxTime = 60;

    [SerializeField] private Image timerBackground; // The background of the timer

    private float timePassed; // Float variable to store how long the game has been running

    [SerializeField] private GameObject creepyDanObj;

    [SerializeField] private GameObject pauseCanvas; // Canvas which will appear when the game is paused
    [SerializeField] private GameObject gameOverCanvas; // Canvas which will appear when the game finishes

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

        // Fill the timer to show how much of the time has passed
        timerBackground.fillAmount = (maxTime - timePassed) / maxTime; 

        // Start the countdown if there are 10 or less seconds remaining and it hasn't already been started
        if (maxTime - timePassed <= 10 && !countdownAnimationStarted)
        {
            countdownAnimationStarted = true;
            numbersTextObj.SetActive(true);
            StartCoroutine(CountdownAnim());
        }

        // Once the time has run out, start the game over function
        if (timePassed >= maxTime && !gameOverCanvas.activeSelf)
        {
            GameOver();
        }
    }

    // A looping coroutine that triggers the animations for the countdown timer for the last 10 seconds as well as changing the text
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

    // Called when the time has run out on the level
    private void GameOver()
    {
        // If the player has won then set up Dan's head to be sad and set up the end screen
        if (GetTotalPower() > 0)
        {
            Time.timeScale = 0;
            creepyDanObj.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(56, 100f);
            winnerText.text = "You Win";
            winnerText.color = greenColour;
            gameOverCanvas.SetActive(true);
        }
        // If the player has lost then set up Dan's head to be happy and set up the end screen
        else
        {
            Time.timeScale = 0;
            creepyDanObj.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(53, 100f);
            winnerText.text = "I Win";
            winnerText.color = redColour;
            gameOverCanvas.SetActive(true);
        }
    }

    // FixedUpdate is called 50 times per second regardless of framerate
    void FixedUpdate()
    {
        // Calculate which player is winning based on the current values of all the devices
        int totalPower = GetTotalPower();

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

    // Get's the current power from all of the devices in the scene and adds them together and returns it
    int GetTotalPower()
    {
        int totalPower = 0;
        foreach (GameObject device in devices)
        {
            totalPower += device.GetComponent<Device>().currentVal;
        }
        return totalPower;
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
