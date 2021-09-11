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
        //boardRotateScript = GameObject.Find("Main Camera").GetComponent<CameraRotation>();
    }

    void OnMouseOver()
    {
        //change cursor to arrows


    }

    void OnMouseDown()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Cursor.visible = false;
            RotationStarted.Invoke();
        }
    }

    void OnMouseUp()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Cursor.visible = true;
            string side = boardRotateScript.RotateToCloseEdge();
            RotationStopped.Invoke(side);
        }
    }

    void OnMouseDrag()
    {
        if (!PauseMenu.GameIsPaused)
        {
            float mouseRotation = Input.GetAxis("Mouse X");
            boardRotateScript.RotateBoard(mouseRotation);
        }
    }
}
