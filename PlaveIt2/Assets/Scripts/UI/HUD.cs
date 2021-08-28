using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
	public Toolbar Toolbar;
	
    // Start is called before the first frame update
    void Start()
    {
		Toolbar.BridgeRemoved += ToolbarScript_ItemRemoved;
    }

	private void ToolbarScript_ItemRemoved(object sender, ToolbarEventArgs e)
	{
		Transform toolbarPanel = transform.Find("BridgesToolbar");
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
