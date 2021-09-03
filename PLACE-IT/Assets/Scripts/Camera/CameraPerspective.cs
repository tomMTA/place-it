using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerspective : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera Camera2dSideA;
    [SerializeField] private Camera Camera2dSideB;
    [SerializeField] private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
        Camera2dSideA.enabled = false;
        Camera2dSideB.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switchPerspective();
        }

        if (!mainCamera.enabled)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                switchBetweenSideCameras();
                //disableMovement
            }

        }
    }

    private void switchPerspective()
    {
        mainCamera.enabled = !mainCamera.enabled;
        if (mainCamera.enabled)
        {
            canvas.worldCamera = mainCamera;
            disableSideCameras();
        }
        else
        {
            Camera2dSideA.enabled = !Camera2dSideA.enabled;
            canvas.worldCamera = Camera2dSideA;
        }
    }

    private void disableSideCameras()
    {
        Camera2dSideA.enabled = false;
        Camera2dSideB.enabled = false;
    }    

    private void switchBetweenSideCameras()
    {
        Camera2dSideA.enabled = !Camera2dSideA.enabled;
        Camera2dSideB.enabled = !Camera2dSideB.enabled;
        if (Camera2dSideB.enabled)
        {
            canvas.worldCamera = Camera2dSideB;
        }
        else
        {
            canvas.worldCamera = Camera2dSideA;
        }
    }
}
