using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerspective : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera Camera2dSideA;
    [SerializeField] private Camera Camera2dSideB;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject winPanel;
    private bool m_IsWin;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
        Camera2dSideA.enabled = false;
        Camera2dSideB.enabled = false;
        m_IsWin = false;
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
            if (m_IsWin)
            {
                winPanel.SetActive(true);
            }

            mainCanvas.worldCamera = mainCamera;
            disableSideCameras();
        }
        else
        {
            if (winPanel.active)
            {
                m_IsWin = true;
                winPanel.SetActive(false);
            }

            Camera2dSideA.enabled = !Camera2dSideA.enabled;
            mainCanvas.worldCamera = Camera2dSideA;
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
            mainCanvas.worldCamera = Camera2dSideB;
        }
        else
        {
            mainCanvas.worldCamera = Camera2dSideA;
        }
    }
}
