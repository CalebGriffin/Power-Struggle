using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the rotation of the Dan object in the scene when the game starts
public class DanAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(-10f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
