using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPivot : MonoBehaviour
{
    BoardRotate boardRotateScript;
    public delegate void RotationStartHandler();
    public RotationStartHandler RotationStarted;
    public delegate void RotationStopHandler(string i_Side);
    public RotationStopHandler RotationStopped;

    void Start()
    {
        boardRotateScript = transform.parent.GetComponent<BoardRotate>();
    }

    void OnMouseOver()
    {
        //change cursor to arrows


    }

    void OnMouseDown()
    {
        Cursor.visible = false;
        RotationStarted.Invoke();
    }

    void OnMouseUp()
    {
        Cursor.visible = true;
        string side = boardRotateScript.RotateToCloseEdge();
        RotationStopped.Invoke(side);
    }

    void OnMouseDrag()
    {
        float mouseRotation = Input.GetAxis("Mouse X");

        boardRotateScript.RotateBoard(mouseRotation);
    }
}
