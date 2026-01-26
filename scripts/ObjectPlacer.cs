using Godot;


public partial class ObjectPlacer : Node
{
	public FurnitureData objectToPlace;
	private Vector3 _targetPosition = Vector3.Zero;
	private Node3D _placedObject;


	public void Init(FurnitureData newFurniture)
	{
		objectToPlace = newFurniture;
		_placedObject = objectToPlace.scene3D.Instantiate<Node3D>();
		
		var collision = Utils.GetFirstChildOfType<CollisionShape3D>(_placedObject);
		if (collision != null) {
			// GD.Print($"Collision disabled");
			collision.Disabled = true;
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		
		InputMaster.Instance.CurrentInputState = InputMaster.InputState.OBJECT_PLACING;

		InputMaster.Instance.OnDragUpdated += DoPlayerMoveInputUpdated;
		InputMaster.Instance.OnTap += DoRotateObject;
		InputMaster.Instance.OnDoubleTap += DoPlaceObject;

		CameraMaster.NodeToFollow = _placedObject;

		GetTree().Root.AddChild(_placedObject);
	}

	public override void _Ready()
	{
		base._Ready();

		_targetPosition = Global.GetPlayerNode().GlobalPosition;
	}

	public void DoPlayerMoveInputUpdated(Vector2 newInput)
	{
		_targetPosition -= CameraMaster.Instance.GetCameraYawQuat() * new Vector3(newInput.X, 0.0f, newInput.Y) * 0.01f;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		var roundedPosition = _targetPosition.Ceil();
		roundedPosition.Y = 0.25f;
		_placedObject.GlobalPosition = _placedObject.GlobalPosition.Lerp(roundedPosition, 20.0f * (float)delta);
	}

	private void DoRotateObject()
	{
		_placedObject.RotateY(Mathf.Pi / 2.0f);
	}

	private void DoPlaceObject()
	{
		var placeTween = _placedObject.CreateTween();
		placeTween.TweenProperty(_placedObject, "position:y", 0, 0.2f);
		placeTween.SetEase(Tween.EaseType.In);
		placeTween.SetTrans(Tween.TransitionType.Elastic);

		var collision = Utils.GetFirstChildOfType<CollisionShape3D>(_placedObject);
		if (collision != null)
			collision.Disabled = false;

		CameraMaster.NodeToFollow = null;

		this.QueueFree();
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		InputMaster.Instance.CurrentInputState = InputMaster.InputState.PLAYER_CONTROLS;

		InputMaster.Instance.OnTap -= DoRotateObject;
		InputMaster.Instance.OnDoubleTap -= DoPlaceObject;
	}

}
