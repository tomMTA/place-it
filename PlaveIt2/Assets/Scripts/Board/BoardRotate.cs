using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    private float mouseAxisX;
    public delegate void SwitchSideHandler();
    public SwitchSideHandler SwitchedSide;

    /*// Start is called before the first frame update
    void Start()
    {
        
    }*/
/*
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
        RotateToCloseEdge();
    }*/

    /*void OnMouseDrag()
    {
        float mouseRotation = Input.GetAxis("Mouse X");

        if (isInLimitsAndCorrect(mouseRotation))
        {
            transform.Rotate(0, 0, -(Input.GetAxis("Mouse X") * rotationSpeed));
        }
    }*/

    public void RotateBoard(float i_MouseRotation)
    {
        if (isInLimitsAndCorrect(i_MouseRotation))
        {
            transform.Rotate(0, -(Input.GetAxis("Mouse X") * rotationSpeed), 0);
        }
    }

    private float distanceFromRight()
    {
        return transform.eulerAngles.y - 0;
    }

    private float distanceFromLeft()
    {
        return 90 - transform.eulerAngles.y;
    }

    public string RotateToCloseEdge() //switch to enum eSide
    {
        string side;

        if (distanceFromLeft() < distanceFromRight())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            side = "B";
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            side = "A";
        }

        return side;
    }

    private bool isExceedingRightLimitAndCorrect(float i_MouseRotation)
    {
        bool isExceedingRightLimit = false;

        if (i_MouseRotation > 0 && (transform.eulerAngles.y == 0 || transform.eulerAngles.y >= 270))
        {
            isExceedingRightLimit = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }

        return isExceedingRightLimit;
    }

    private bool isExceedingLeftLimitAndCorrect(float i_MouseRotation)
    {
        bool isExceedingLeftLimit = false;

        if (i_MouseRotation < 0 && transform.eulerAngles.y >= 90)
        {
            isExceedingLeftLimit = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }

        return isExceedingLeftLimit;
    }

    private bool isInLimitsAndCorrect(float i_MouseRotation)
    {
        bool isInLimits = true;

        if (isExceedingRightLimitAndCorrect(i_MouseRotation) || isExceedingLeftLimitAndCorrect(i_MouseRotation))
        {
            isInLimits = false;
        }

        return isInLimits;
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
