using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InentoryManager : MonoBehaviour
{
	// public GameObject activeBridge;
	[HideInInspector] public GameObject activeBridge;

	[SerializeField] List<GameObject> bridgeButtons;

	public void BridgeButtonClicked(GameObject bridge)
	{
		Debug.Log("Received notification");
		if (!bridge.activeInHierarchy)
		{
			if(activeBridge)
			{
				activeBridge.SetActive(false);
			}
			activeBridge = bridge;
			setPositionOnActivated(activeBridge);
			activeBridge.SetActive(true);
			//activeBridge = bridge;
		}
		//bridge.Enabled = !bridge.Enabled;
	}

	private void setPositionOnActivated(GameObject activeBridge)
	{
		activeBridge.transform.position = new Vector3(55f, 100f, 63f); 
	}

	void Start()
    {
        foreach (GameObject border in bridgeButtons)
		{
			border.GetComponent<ClickOnBridge>().bridgeButtonClicked += BridgeButtonClicked;
		}
    }

	private void OnDestroy()
	{
		Debug.Log("OnDestroy for InventoryManager was invoked");
		foreach (GameObject border in bridgeButtons)
		{
			border.GetComponent<ClickOnBridge>().bridgeButtonClicked -= BridgeButtonClicked;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
