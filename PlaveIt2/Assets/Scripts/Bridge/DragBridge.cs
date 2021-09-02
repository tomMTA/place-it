
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBridge : MonoBehaviour
{
	[SerializeField] private float xVelocity = 7f;
	[SerializeField] private float zVelocity = 10f;
	private SlotsHighlighter slotsHighlighter;
	Vector3 mouseAsWorldPosition;
	Vector3 objectPosition;
	private float mouseAxisX;
	private float mousePositionY;
	private float mouseAxisY;
	private static bool isClicked = false;
	private bool m_IsTilted;
	private const float rangeFromBoard = 100;
	private GameObject gameBoard; //redundant to hold as member? maybe just create to exctract the Vector3?
	private Vector3 boardPosition; //redundant to hold as member? maybe just create to calculate the boundaries?
	private float[] xBounadries = new float[2];
	private float[] zBoundaries = new float[2];
	public delegate void TiltedHandler();
	public event TiltedHandler Tilted;

	void Start()
    {
		slotsHighlighter = transform.GetChild(0).GetComponent<SlotsHighlighter>();
		m_IsTilted = false;
		gameBoard = GameObject.FindGameObjectWithTag("GameBoard");
		MeshRenderer boardRenderer = gameBoard.GetComponentsInChildren<MeshRenderer>()[0]; //PLASTER - if we add more renderers we need to modify this
		boardPosition = boardRenderer.bounds.center;
		xBounadries[0] = boardPosition.x - rangeFromBoard;
		xBounadries[1] = boardPosition.x + rangeFromBoard;
		zBoundaries[0] = boardPosition.z - rangeFromBoard;
		zBoundaries[1] = boardPosition.z + rangeFromBoard;
	}

	public bool IsTilted
    {
        get { return m_IsTilted; }
    }

	void OnMouseUp()
	{
		Cursor.visible = true;
		isClicked = false;
		HoverHighlight HoverHighlightScriptReference = GetComponent<HoverHighlight>();
		HoverHighlightScriptReference.ResetColor();
	}

	void OnMouseDown()
	{
		Cursor.visible = false;
		isClicked = true;
	}

	void OnMouseDrag()
	{
		if (enabled)
		{
			mouseAxisX = Input.GetAxis("Mouse X");
			mouseAxisY = Input.GetAxis("Mouse Y");
			float newX = transform.position.x + mouseAxisX * xVelocity;
			float newZ = transform.position.z + mouseAxisY * zVelocity;

			if (!isInXRange(newX))
			{
				newX = transform.position.x;
			}
			if (!isInZRange(newZ))
			{
				newZ = transform.position.z;
			}
			transform.position = new Vector3(newX, transform.position.y, newZ);

			/*if (Input.GetMouseButtonDown(1))
			{
				Rotate90Degree();
			}*/
		}
	}

	/*void OnMouseOver()
	{
		if (enabled && !isClicked && Input.GetMouseButtonDown(1))
		{
			Rotate90Degree();
		}
	}

	private void Rotate90Degree()
	{
		int newYAngle = transform.eulerAngles.y == 180 ? 270 : 180;
		Tilted.Invoke();
		m_IsTilted = !m_IsTilted;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYAngle, transform.eulerAngles.z);
		//slotsHighlighter.IsTilted = m_IsTilted;
	}*/

	//improve performance - calculate posToVerify once
	private bool isInXRange(float newX)
    {
		return (newX >= xBounadries[0]) && (newX <= xBounadries[1]);
	}

	private bool isInZRange(float newZ)
	{
		return (newZ >= zBoundaries[0]) && (newZ <= zBoundaries[1]);
	}
}