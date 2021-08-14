using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] GameObject m_Board;

    void Start()
    {
        transform.LookAt(m_Board.transform);
    }

    public void RotateAround(float i_MouseRotation)
    {
        transform.LookAt(m_Board.transform);
        transform.Translate(Vector3.left * i_MouseRotation * rotationSpeed);
    }
}
