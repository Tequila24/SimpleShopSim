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
	private Vector3 _cameraPosition = Vector3.One;



	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	public override void _Ready()
	{	
		_cameraPosition = _mainCamera.Position;
		InputMaster.Instance.OnZoomCamera += DoCameraZoom;

		NodeToFollow = GetTree().GetNodesInGroup("Player")[0] as Node3D;
	}

	public void DoCameraZoom(float zoomDelta)
	{
		_cameraPosition.Z = Mathf.Clamp(_cameraPosition.Z + zoomDelta * 0.1f, 4.0f, 10.0f);
		GD.Print($"new camera position {_cameraPosition}");
	}

	public override void _Process(double delta)
	{
		if (NodeToFollow == null)
			return;
		
		_cameraDolly.Position = _cameraDolly.Position.Lerp(NodeToFollow.GlobalPosition, 5.0f * (float)delta);
		_mainCamera.Position = _mainCamera.Position.Lerp(_cameraPosition, 10.0f * (float)delta);
	}

	static public Camera3D GetCurrentCamera()
	{
		return Instance._mainCamera;
	}

	public Quaternion GetCameraYawQuat()
	{
		// return Quaternion.Identity;
		// Quaternion cameraYawRotation = Quaternion.Identity;
		// camera
		return Quaternion.FromEuler(new Vector3(0, (float)Math.PI+_cameraDolly.Rotation.Y, 0));
	}
}
