using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBridge : MonoBehaviour
{
	[SerializeField] public GameObject bridgeRef;

	public delegate void BridgeButtonClickedHandler(GameObject bridge);
	public event BridgeButtonClickedHandler bridgeButtonClicked;

	public void BridgeButtonClicked()
	{
		bridgeButtonClicked?.Invoke(bridgeRef);
		//bridgeRef.SetActive(!bridgeRef.activeInHierarchy);
	}

	public Bridge BridgeScript
    {
		get { return bridgeRef.GetComponent<Bridge>(); }
    }
}
