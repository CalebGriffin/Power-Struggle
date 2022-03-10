using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the scene transition animations
public class LevelLoader : MonoBehaviour
{
    [SerializeField] protected GameObject TransitionObj; // The object to animate on screen
    [SerializeField] protected float transitionTime = 0.5f; // How long should the transition take

    // When the scene loads, play the opening animation. Virtual so it can be overridden
    virtual protected void OnEnable()
    {
        TransitionObj.GetComponent<Animator>().Play("CircleWipe_End");
    }

    // Calls the Coroutine to transition to another scene, takes the name of the scene as a parameter, public so it can be accessed by other classes
    public void StartTransition(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    // Coroutine to transition between scenes, takes the name of the scene as a parameter, only called by StartTransition()
    private IEnumerator TransitionToScene(string sceneName)
    {
        // Set the animation object to visible and start the animation
        TransitionObj.SetActive(true);
        TransitionObj.GetComponent<Animator>().Play("CircleWipe_Start");

        // Reset the Time scale back to normal time
        Time.timeScale = 1;

        // Wait for the transition time
        yield return new WaitForSeconds(transitionTime);

        // If the player is returning to the menu, then set the relevant boolean to true
        if (sceneName == "Menu Scene")
        {
            gVar.backToMenuUI = true;
        }

        // Load the scene using the Scene Manager and log the result for testing
        SceneManager.LoadScene(sceneName);
        Debug.Log("Level Loader " + sceneName);
    }
}