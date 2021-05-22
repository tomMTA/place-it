using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    private float mouseAxisX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseOver()
    {
        //change cursor to arrows


    }

    void OnMouseDown()
    {
        Cursor.visible = false;
    }

    void OnMouseUp()
    {
        Cursor.visible = true;
    }

    void OnMouseDrag()
    {
        transform.Rotate(0, 0, -(Input.GetAxis("Mouse X") * rotationSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
