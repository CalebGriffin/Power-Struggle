using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] protected GameObject TransitionObj;
    [SerializeField] protected float transitionTime = 0.5f;

    virtual protected void OnEnable()
    {
        TransitionObj.GetComponent<Animator>().Play("CircleWipe_End");
    }

    public void StartTransition(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        TransitionObj.SetActive(true);

        TransitionObj.GetComponent<Animator>().Play("CircleWipe_Start");

        Time.timeScale = 1;
        Debug.Log("Level Loader Getting Here 1");

        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Level Loader Getting Here 2");

        if (sceneName == "Menu Scene")
        {
            gVar.backToMenuUI = true;
        }

        SceneManager.LoadScene(sceneName);
        Debug.Log("Level Loader " + sceneName);
    }
}
