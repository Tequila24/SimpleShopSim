using Godot;
using System;

public partial class CameraMaster : Node
{
	public static CameraMaster Instance
	{
		private set; get;
	}
	
	[Export]
	private Camera3D _mainCamera;
	[Export]
	private Node3D _cameraDolly;

	public Node3D NodeToFollow;




	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	public override void _Ready()
	{	
		NodeToFollow = GetTree().GetNodesInGroup("Player")[0] as Node3D;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (NodeToFollow == null)
			return;
		
		_cameraDolly.Position = _cameraDolly.Position.Lerp(NodeToFollow.GlobalPosition, 5.0f * (float)delta);
	}

	public Quaternion GetCameraYawQuat()
	{
		// return Quaternion.Identity;
		// Quaternion cameraYawRotation = Quaternion.Identity;
		// camera
		return Quaternion.FromEuler(new Vector3(0, (float)Math.PI+_cameraDolly.Rotation.Y, 0));
	}
}
