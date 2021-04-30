
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBridge : MonoBehaviour
{
	[SerializeField] private float xVelocity = 7f;
	[SerializeField] private float zVelocity = 10f;

	Vector3 mouseAsWorldPosition;
	Vector3 objectPosition;
	private float mouseAxisX;
	private float mousePositionY;
	private float mouseAxisY;
	private static bool isClicked = false;
	private const float rangeFromBoard = 100;
	private GameObject gameBoard; //redundant to hold as member? maybe just create to exctract the Vector3?
	private Vector3 boardPosition; //redundant to hold as member? maybe just create to exctract the Vector3?
	private float[] xBounadries = new float[2];
	private float[] zBoundaries = new float[2];

	void Start()
    {
		gameBoard = GameObject.Find("GameBoard");
		MeshRenderer boardRenderer = gameBoard.GetComponentsInChildren<MeshRenderer>()[0]; //plaster - if we add more renderers we need to modify this
		boardPosition = boardRenderer.bounds.center;
		xBounadries[0] = boardPosition.x - rangeFromBoard;
		xBounadries[1] = boardPosition.x + rangeFromBoard;
		zBoundaries[0] = boardPosition.z - rangeFromBoard;
		zBoundaries[1] = boardPosition.z + rangeFromBoard;
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

		if (Input.GetMouseButtonDown(1))
		{
			Rotate90Degree();
		}
	}

	void OnMouseOver()
	{
		if (!isClicked && Input.GetMouseButtonDown(1))
		{
			Rotate90Degree();
		}
	}

	private void Rotate90Degree()
	{
		transform.rotation *= Quaternion.Euler(0, 0, 90f);
	}

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