using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private LineRenderer lr;
    private BoxCollider colliderHit;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = {new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position};
        lr = GetComponent<LineRenderer>();
        lr.SetPositions(points);
        colliderHit = GetComponent<BoxCollider>();

    }

    void FixedUpdate()
    {
        Vector3 worldPos = transform.InverseTransformPoint(lr.GetPosition(1));
        colliderHit.center = worldPos;
    }

    public void BoxOn()
    {
        colliderHit.enabled = true;
    }

    public void BoxOff()
    {
        colliderHit.enabled = false;
    }
}
