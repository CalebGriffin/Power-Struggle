using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private RaycastHit hit;
    public Vector3 pos;
    private GameObject currentOrb = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Shoot a ray from the current touch coorditnaets
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Orb")
                {
                    pos = hit.point;
                    //hit.collider.gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(touch.position.x, 0f, touch.position.y));
                    //hit.collider.gameObject.GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position + new Vector3(pos.x, 0.1f, pos.z + 4.2f));
                    hit.collider.gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(pos.x, 0.1f, pos.z));
                }
            }
        }*/

        if (Input.touches != null)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Orb")
                {
                    pos = hit.point;
                    currentOrb = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(pos.x, 0.1f, pos.z));
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    pos = hit.point;

                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(pos.x, 0.1f, pos.z));
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Device")
                {
                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, 0.1f, hit.transform.position.z));
                        currentOrb = null;
                    }
                }
                else
                {
                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(currentOrb.transform.position.x, 0.1f, currentOrb.transform.position.z));
                        currentOrb = null;
                    }
                }
            }
        }
        
    }
}
