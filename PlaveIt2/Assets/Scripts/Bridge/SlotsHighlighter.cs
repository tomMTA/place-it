using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - adjust accuracy of rays. debug.raycast works fine but is a bit off
//       find out why raycast isnt working if so

public class SlotsHighlighter : MonoBehaviour
{
    private GameObject currentSlot;
    //public GameObject left, right;

    // Start is called before the first frame update
    void Start()
    {
        currentSlot = null;
    }
        //Debug.Log("LocalPosition: " + this.GetComponent<Transform>().position);
        //Debug.Log("GlobalPosition: " + this.transform.position);

    // Update is called once per frame
    void FixedUpdate()
    {
	    Debug.DrawRay(transform.position, Vector3.down * 10000, Color.white, 0f, false);
        //Ray rayLeft = Camera.main.ScreenPointToRay(left.transform.position);
        //Ray rayRight = Camera.main.ScreenPointToRay(right.transform.position);
        RaycastHit hit;
        Vector3 relativePosition = new Vector3(104.25f, 20f, 0f);
        //Debug.Log("At " + transform.position); //positions are not compatible - probably hierarchy related
        if (Physics.Raycast(relativePosition, Vector3.down, out hit, Mathf.Infinity)) //(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            if (hit.collider != null)
            {
                GameObject hitReceiver = hit.collider.gameObject;
                Debug.Log(hitReceiver.transform.tag);
                if (hitReceiver.tag == "Slot" && hitReceiver != currentSlot)
                {
	                Debug.Log("Hit slot at " + hitReceiver.transform.position + "!");
                    if (currentSlot != null)
                    {
                        currentSlot.GetComponent<MeshRenderer>().enabled = false;
                    }
                    hitReceiver.GetComponent<MeshRenderer>().enabled = false;
                    currentSlot = hitReceiver;
                }
                if (hitReceiver == null)
                    Debug.Log("no object obtained from hit collider");
            }
            else
                Debug.Log("no collider detcted");
        }
        else Debug.Log("no object returned from RayCast");

        /*if (Physics.Raycast(rayRight, out hit, 1000f)) //(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            if (hit.collider != null)
            {
                GameObject hitReceiver = hit.collider.gameObject;
                if (hitReceiver.tag == "Slot" && hitReceiver != currentSlot)
                {
                    if (currentSlot != null)
                    {
                        currentSlot.GetComponent<MeshRenderer>().enabled = false;
                    }
                    hitReceiver.GetComponent<MeshRenderer>().enabled = true;
                    currentSlot = hitReceiver;
                }
                if (hitReceiver == null)
                    Debug.Log("no object obtained from hit collider");
            }
            else
                Debug.Log("no collider detcted");
        }
        else Debug.Log("no object returned from RayCast");*/
    }
}