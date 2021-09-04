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
		Debug.Log("Click on bvridge");
		bridgeButtonClicked?.Invoke(bridgeRef);
		//bridgeRef.SetActive(!bridgeRef.activeInHierarchy);
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
