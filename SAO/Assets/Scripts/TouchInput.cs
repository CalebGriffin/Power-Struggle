using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private RaycastHit hit; // To take the output info from the Raycast
    public Vector3 pos; // A temporary variable to store the position of the touch
    private GameObject currentOrb = null; // The current orb that the player is dragging

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is touching the screen with at least one finger
        if (Input.touches.Length > 0)
        {
            // Get the touch info for the first finger that touched the screen
            Touch touch = Input.touches[0];

            // True on the first frame that the player's touch is detected
            if (touch.phase == TouchPhase.Began)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // If the Raycast has hit an Orb, set the position of the Line Renderer and disable the hitbox on the Orb
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Orb")
                {
                    pos = hit.point;
                    currentOrb = hit.collider.gameObject;
                    currentOrb.SendMessage("BoxOff");
                    hit.collider.gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(pos.x, 0.1f, pos.z));
                }
            }
            // True every frame when the player's finger is still on the screen
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // If the Raycast hit's something, move the Line Renderer and disale the hitbox on the Orb
                if (Physics.Raycast(ray, out hit))
                {
                    pos = hit.point;

                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(pos.x, 0.1f, pos.z));
                        currentOrb.SendMessage("BoxOff");
                    }
                }
            }
            // True on the frame when the player's touch is no longer detected
            else if (touch.phase == TouchPhase.Ended)
            {
                // Shoot a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // If the player lifted their finger on a Device, connect the Line Renderer to that device and reenable the hitbox on the Orb
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Device")
                {
                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, 0.1f, hit.transform.position.z));
                        currentOrb.SendMessage("BoxOn");
                        currentOrb = null;
                    }
                }
                // If the player didn't lift their finger on a Device, reset the Line Renderer of the Orb
                else
                {
                    if (currentOrb != null)
                    {
                        currentOrb.GetComponent<LineRenderer>().SetPosition(1, new Vector3(currentOrb.transform.position.x, 0.1f, currentOrb.transform.position.z));
                        currentOrb.SendMessage("BoxOff");
                        currentOrb = null;
                    }
                }
            }
        }
        
    }
}
