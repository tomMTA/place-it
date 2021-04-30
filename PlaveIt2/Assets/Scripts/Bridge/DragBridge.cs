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

		transform.position += new Vector3(mouseAxisX * xVelocity, 0, mouseAxisY * zVelocity);
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
}