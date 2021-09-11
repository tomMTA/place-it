using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar : MonoBehaviour 
{
	private const int SLOTS = 9;
	private List<IToolbarBridge> levelBridges = new List<IToolbarBridge>();
	public event EventHandler<ToolbarEventArgs> bridgeReturned;
	public event EventHandler<ToolbarEventArgs> BridgeRemoved;
	
	public void returnBridge(IToolbarBridge bridge)
	{
		if(levelBridges.Count < SLOTS)
		{
			Collider collider = (bridge as MonoBehaviour).GetComponent<Collider>();
			if(collider.enabled)
			{
				collider.enabled = false;
				levelBridges.Add(bridge);
				bridge.OnReturn();

				if (bridgeReturned != null)
				{
					bridgeReturned(this, new ToolbarEventArgs(bridge));
				}
			}
		}
	}

	public void removeBridge(IToolbarBridge bridge)
	{
		if (levelBridges.Contains(bridge))
		{
			levelBridges.Remove(bridge);
			bridge.OnDrop();
			if(BridgeRemoved != null)
			{
				BridgeRemoved(this, new ToolbarEventArgs(bridge));
			}
		}
	}
}
