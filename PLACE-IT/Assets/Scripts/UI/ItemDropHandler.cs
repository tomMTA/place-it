using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler, IToolbarBridge
{
	public string Name => throw new System.NotImplementedException();

	public Sprite Image => throw new System.NotImplementedException();

	public void OnDrop(PointerEventData eventData)
	{
		RectTransform invPanel = transform as RectTransform;
		if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
		{
			Debug.Log("Drop item");
		}
	}

	public void OnDrop()
	{
		throw new System.NotImplementedException();
	}

	public void OnReturn()
	{
		throw new System.NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
