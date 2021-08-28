using System;
using UnityEngine;

public interface IToolbarBridge
{
	string Name { get; }

	Sprite Image { get; }

	void OnReturn();

	void OnDrop();
}

public class ToolbarEventArgs : EventArgs
{
	IToolbarBridge Bridge;

	public ToolbarEventArgs (IToolbarBridge bridge)
	{
		Bridge = bridge;
	}
}
