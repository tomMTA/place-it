using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBridge : MonoBehaviour
{
	[SerializeField] public GameObject bridgeRef;

    public void BridgeButtonClicked()
	{
		bridgeRef.SetActive(!bridgeRef.activeInHierarchy);
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
