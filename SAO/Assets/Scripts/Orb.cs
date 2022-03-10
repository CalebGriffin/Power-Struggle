using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private LineRenderer lr; // Reference to the line renderer which shows the orb's connection
    private BoxCollider colliderHit; // Used to detect when connected to a device

    // Start is called before the first frame update
    void Start()
    {
        // Create the points for the Line Renderer, get the Line Renderer and set up the points
        Vector3[] points = {new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position};
        lr = GetComponent<LineRenderer>();
        lr.SetPositions(points);

        // Get reference to the Box Collider
        colliderHit = GetComponent<BoxCollider>();
    }

    // Set's the position of the Box Collider to the point on the Line Renderer by converting World Space to Local Space
    void FixedUpdate()
    {
        Vector3 worldPos = transform.InverseTransformPoint(lr.GetPosition(1));
        colliderHit.center = worldPos;
    }

    // Disconnect the Orb from the device
    public void Reset()
    {
        lr.SetPosition(1, transform.position);
    }

    // Enable the Box Collider
    public void BoxOn()
    {
        colliderHit.enabled = true;
    }

    // Disable the Box Collider
    public void BoxOff()
    {
        colliderHit.enabled = false;
    }
}