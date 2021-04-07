using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public int height = 1;
    public int spacesOccupies = 2;
    public int id;

    public Transform bridge;
    private float bridgeAngle = 5f;
    public float bridgeRotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        initBridge();    
    }

    // Update is called once per frame
    void Update()
    {
        RotateOnClickAndDrag();
    }

    private void initBridge()
    {
    }

    private void RotateOnClickAndDrag()
    {
        bridgeAngle += Input.GetAxis("Mouse X") * bridgeRotationSpeed * -Time.deltaTime;
        bridgeAngle = Mathf.Clamp(bridgeAngle, 0, 180);
        //if (Input.GetMouseButtonDown(0));
        //{
        //    this.transform.Rotate(Time.deltaTime * 10);
        //}
    }
}
