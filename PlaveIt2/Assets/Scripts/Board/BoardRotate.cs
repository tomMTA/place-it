using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotate : MonoBehaviour
{
    //[SerializeField] private float rotationSpeed = 50f;
    private float rotationSpeed = 500f;
    private const string BoardPivotPointName = "BoardPivotPoint";
    private GameObject BoardPivotPoint;
    [SerializeField] private GameObject _default;
    private float rotationZ;


    // Start is called before the first frame update
    void Start()
    {
	    BoardPivotPoint = GameObject.Find(BoardPivotPointName);
    }

    void OnMouseOver()
    {
        //change cursor to arrows


    }

    void OnMouseDown()
    {
        //Cursor.visible = false;
    }

    void OnMouseUp()
    {
        //Cursor.visible = true;
    }

    void OnMouseDrag()
    {
	    float newAngle = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
		_default.transform.RotateAround(BoardPivotPoint.transform.position, Vector3.down, newAngle);
    }

	// Update is called once per frame
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.B))
	    {
		    GameObject.Find("BlueBridge1").transform.parent = this.transform;
	    }
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("transform.rotation angles x: " + _default.transform.eulerAngles.x + " y: " + _default.transform.eulerAngles.y + " z: " + _default.transform.eulerAngles.z);
			Debug.Log(Input.GetAxis("Mouse X"));
		}
	}
}
