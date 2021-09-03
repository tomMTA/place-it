using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldDragBridge : MonoBehaviour
{
    Vector3 mouseAsWorldPosition;
    Vector3 objectPosition;
    private float mousePositionX;
    private float mousePositionY;
    private float mousePositionZ;

    void OnMouseUp()
    {
        HoverHighlight HoverHighlightScriptReference = GetComponent<HoverHighlight>();
        HoverHighlightScriptReference.ResetColor();
    }

    void OnMouseDown()
    {
        objectPosition = transform.position;
        mouseAsWorldPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePositionX = Input.mousePosition.x - mouseAsWorldPosition.x;
        mousePositionZ = Input.mousePosition.y - mouseAsWorldPosition.y;
        mousePositionY = Input.mousePosition.z - mouseAsWorldPosition.z;
    }

    void OnMouseDrag()
    {
        float distanceX = Input.mousePosition.x - mousePositionX;
        float distanceY = Input.mousePosition.y - mousePositionZ;
        float distanceZ = Input.mousePosition.z - mousePositionY;
        Vector3 finalPosition = Camera.main.ScreenToWorldPoint(new Vector3(distanceX, distanceY, distanceZ));
        transform.position = new Vector3(finalPosition.x, objectPosition.y, finalPosition.z);
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //do stuff here
            transform.rotation *= Quaternion.Euler(0, 0, 90f);
        }
    }
}