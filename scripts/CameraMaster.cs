using Godot;
using System;

public partial class CameraMaster : Node
{
	public static CameraMaster Instance
	{ private set; get; }
	
	[Export]
	private Camera3D _mainCamera;
	[Export]
	private Node3D _cameraDolly;

	private Node3D _nodeToFollow;
	static public Node3D NodeToFollow
	{
		get { return Instance._nodeToFollow; }
		set { if (value != null) Instance._nodeToFollow = value; else Instance._nodeToFollow = Instance.GetTree().GetNodesInGroup("PlayerGroup")[0] as Node3D; }
	}

	private Vector3 _targetCameraPosition = Vector3.One;
	// private float _targetCameraRotation = 0;

	public Action OnCameraRotated;
	public float CameraYawAngle => _cameraDolly.RotationDegrees.Y;



	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	public override void _Ready()
	{	
		_targetCameraPosition = _mainCamera.Position;
		// _targetCameraRotation = _cameraDolly.Rotation.Y;
		
		InputMaster.Instance.OnZoomCamera += DoZoomCamera;
		InputMaster.Instance.OnRotateCamera += DoRotateCamera;

		_nodeToFollow = GetTree().GetNodesInGroup("PlayerGroup")[0] as Node3D;
	}

	public void DoZoomCamera(float zoomDelta)
	{
		_targetCameraPosition.Z = Mathf.Clamp(_targetCameraPosition.Z + zoomDelta * 0.1f, 4.0f, 10.0f);
		// GD.Print($"new camera position {_targetCameraPosition}");
	}

	public void DoRotateCamera(float rotateDelta)
	{
		_cameraDolly.RotateY(-rotateDelta * 0.003f);
		OnCameraRotated?.Invoke();
	}

	public override void _Process(double delta)
	{
		if (_nodeToFollow == null)
			return;
		
		_cameraDolly.Position = _cameraDolly.Position.Lerp(_nodeToFollow.GlobalPosition, 5.0f * (float)delta);
		_mainCamera.Position = _mainCamera.Position.Lerp(_targetCameraPosition, 10.0f * (float)delta);
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
