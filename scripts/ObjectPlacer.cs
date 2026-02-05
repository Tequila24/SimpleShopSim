using Godot;


public partial class ObjectPlacer : Node
{
	public FurnitureData objectToPlace;
	private Vector3 _targetPosition = Vector3.Zero;
	private Node3D _placedObject;


	public void InitWithPrefab(FurnitureData newFurniture)
	{
		objectToPlace = newFurniture;
		_placedObject = objectToPlace.scene3D.Instantiate<Node3D>();
		_targetPosition = Global.GetPlayerNode().GlobalPosition + new Vector3(0, 0.25f, 0);
		
		var collision = Utils.GetFirstChildOfType<CollisionShape3D>(_placedObject);
		if (collision != null) {
			// GD.Print($"Collision disabled");
			collision.Disabled = true;
		}
	}

	public void InitWithExistingObject(Node3D _movedObject)
	{
		objectToPlace = null;
		_placedObject = _movedObject;
		_targetPosition = _movedObject.Position;
		var raiseTween = _movedObject.CreateTween();
		raiseTween.TweenProperty(this, "_targetPosition:y", 0.25f, 0.2f);


		
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

		if (_placedObject.GetParent() != LevelMaster.Instance)
			LevelMaster.Instance.AddChild(_placedObject);
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public void DoPlayerMoveInputUpdated(Vector2 newInput)
	{
		_targetPosition -= CameraMaster.Instance.GetCameraYawQuat() * new Vector3(newInput.X, 0.0f, newInput.Y) * 0.01f;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		var roundedPosition = _targetPosition.Ceil();
		_placedObject.GlobalPosition = _placedObject.GlobalPosition.Lerp(new Vector3(
			Mathf.Ceil(_targetPosition.X),
			_targetPosition.Y,
			Mathf.Ceil(_targetPosition.Z)
			),
			 20.0f * (float)delta);
	}

	private void DoRotateObject()
	{
		_placedObject.RotateY(Mathf.Pi / 2.0f);
	}

	private void DoPlaceObject()
	{
		_placedObject.GlobalPosition = new Vector3(
			Mathf.Ceil(_targetPosition.X),
			_targetPosition.Y,
			Mathf.Ceil(_targetPosition.Z)
			);

		var placeTween = _placedObject.CreateTween();
		placeTween.TweenProperty(_placedObject, "position:y", 0, 0.2f);
		placeTween.SetEase(Tween.EaseType.In);
		placeTween.SetTrans(Tween.TransitionType.Elastic);
		placeTween.TweenCallback(Callable.From(
			() => {
				// GD.Print($"CALLING MY ASS");
				SignalBus.OnFurniturePlaced(_placedObject);
				this.QueueFree();
				}
		));

		var collision = Utils.GetFirstChildOfType<CollisionShape3D>(_placedObject);
		if (collision != null)
			collision.Disabled = false;

		CameraMaster.NodeToFollow = null;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		InputMaster.Instance.CurrentInputState = InputMaster.InputState.PLAYER_CONTROLS;

		InputMaster.Instance.OnTap -= DoRotateObject;
		InputMaster.Instance.OnDoubleTap -= DoPlaceObject;
	}

}
