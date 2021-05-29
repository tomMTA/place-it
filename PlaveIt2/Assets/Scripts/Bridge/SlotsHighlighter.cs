using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - adjust accuracy of rays. debug.raycast works fine but is a bit off
//       find out why raycast isnt working if so

public class SlotsHighlighter : MonoBehaviour
{
    private GameObject currentSlot;
    private readonly int k_HighlightSlotLayer = 10;
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject otherHighlighter;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray(transform.position, Vector3.down);
    }
        //Debug.Log("LocalPosition: " + this.GetComponent<Transform>().position);
        //Debug.Log("GlobalPosition: " + this.transform.position);

    void debugRays()
    {
        //for debug--------------------------------------
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }

    void highlightSlots()
    {
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider != null)
            {
                GameObject hitReceiver = hitInfo.collider.gameObject;
                if (hitReceiver.layer == k_HighlightSlotLayer && hitReceiver != currentSlot)
                {
                    if (currentSlot != null)
                    {
                        currentSlot.GetComponent<MeshRenderer>().enabled = false;
                    }
                    hitReceiver.GetComponent<MeshRenderer>().enabled = true;
                    currentSlot = hitReceiver;
                }
            }
        }
        else
        {
            if (currentSlot != null)
            {
                currentSlot.GetComponent<MeshRenderer>().enabled = false;
            }
            currentSlot = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ray.origin = transform.position;
        //debugRays();
        highlightSlots();
    }
}