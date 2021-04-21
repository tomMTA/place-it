using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class DragObject : MonoBehaviour
{
    Vector3 mouseAsWorldPosition;
    Vector3 objectPosition;
    private float mousePositionX;
    private float mousePositionY;
    private float mousePositionZ;
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


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DragObject : MonoBehaviour
//{
//    private Vector3 mOffset;
//    private float mZCoord;

//    void OnMouseDown()
//    {
//        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
//        // Store offset = gameobject world pos - mouse world pos
//        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
//    }

//    private Vector3 GetMouseAsWorldPoint()
//    {
//        // Pixel coordinates of mouse (x,y)
//        Vector3 mousePoint = Input.mousePosition;
//        // z coordinate of game object on screen
//        mousePoint.z = mZCoord;
//        // Convert it to world points
//        return Camera.main.ScreenToWorldPoint(mousePoint);
//    }

//    void OnMouseDrag()
//    {
//        transform.position = GetMouseAsWorldPoint() + mOffset;
//    }
//}