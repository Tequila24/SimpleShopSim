using Godot;
using System;

public partial class FlipSignLogic : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (FindChild("Interactive") is Interactive interactive)
		{
			interactive.OnEntered += (Node3D enteredNode) => { GD.Print($"Flip sign area entered by {enteredNode.Name}"); };
			interactive.OnExited += (Node3D enteredNode) => { GD.Print($"Flip sign area exited by {enteredNode.Name}"); };
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
