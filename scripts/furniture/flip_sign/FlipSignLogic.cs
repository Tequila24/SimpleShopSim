using Godot;
using System;

public partial class FlipSignLogic : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (FindChild("Interactive") is Interactive interactive)
		{
			// interactive.OnEntered += (Node3D enteredNode) => { GD.Print($"Flip sign area entered by {enteredNode.Name}"); };
			// interactive.OnExited += (Node3D enteredNode) => { GD.Print($"Flip sign area exited by {enteredNode.Name}"); };
		}
	}
}
