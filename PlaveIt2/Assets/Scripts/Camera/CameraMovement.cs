using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            transform.position += Vector3.up; //Vector3.forward;//new Vector3(0,1f,1f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            transform.position -= Vector3.up;//Vector3.forward; //new Vector3(0, 1f, 1f);
        }
    }
}
