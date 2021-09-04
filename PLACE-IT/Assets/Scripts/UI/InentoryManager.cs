using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InentoryManager : MonoBehaviour
{
	// public GameObject activeBridge;
	[HideInInspector] public GameObject activeBridge;
	[SerializeField] List<GameObject> bridgeButtons;

	void Start()
	{
		foreach (GameObject border in bridgeButtons)
		{
			ClickOnBridge clickOnBridgeScript = border.GetComponent<ClickOnBridge>();
			clickOnBridgeScript.bridgeButtonClicked += BridgeButtonClicked;
			clickOnBridgeScript.BridgeScript.EnteredSlot += ActiveBridgeEnteredBoard;
			clickOnBridgeScript.BridgeScript.ExitedSlot += BridgeExitedtSlot;
		}
	}

	public void BridgeButtonClicked(GameObject bridge)
	{
		Bridge bridgeScript = bridge.GetComponent<Bridge>();
		if (!bridge.activeInHierarchy)
		{
			if (activeBridge)
			{
				Bridge activeBridgeScript = activeBridge.GetComponent<Bridge>();
				activeBridgeScript.UnHighlightSlots(activeBridgeScript.LeftSlot);
				activeBridge.SetActive(false);
			}
			activeBridge = bridge;
			setPositionOnActivated(activeBridge);
			activeBridge.SetActive(true);
			//activeBridge = bridge;
		}
		//bridge.Enabled = !bridge.Enabled;
	}

	public void ActiveBridgeEnteredBoard()
    {
		Debug.Log("Active bridge is being nulled");
		activeBridge = null;
    }

	public void BridgeExitedtSlot(GameObject i_Bridge)
    {
		if (activeBridge)
		{
			Debug.Log("Active bridge is not null");
			i_Bridge.SetActive(false);
        }
        else
        {
			Debug.Log("Active bridge is null");
			activeBridge = i_Bridge;
			setPositionOnActivated(activeBridge);
        }
    }

	private void setPositionOnActivated(GameObject activeBridge)
	{
		activeBridge.transform.position = new Vector3(55f, 100f, 63f); 
	}

	private void OnDestroy()
	{
		Debug.Log("OnDestroy for InventoryManager was invoked");
		foreach (GameObject border in bridgeButtons)
		{
			border.GetComponent<ClickOnBridge>().bridgeButtonClicked -= BridgeButtonClicked;
		}
	}
}
