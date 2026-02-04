using Godot;


public partial class DoorLogic : Node3D
{
	[Export]
	private Area3D _detectionArea;
	[Export]
	private Node3D _doorLeft;
	[Export]
	private Node3D _doorRight;
	
	private int _currentCount = 0;

	private Vector3 _leftClosedPosition = Vector3.Zero;
	private Vector3 _rightClosedPosition = Vector3.Zero;
	private Tween _doorTween = null;



	public override void _Ready()
	{
		_detectionArea.BodyEntered += DoBodyEntered;
		_detectionArea.BodyExited += DoBodyExited;

		_leftClosedPosition = _doorLeft.Position;
		_rightClosedPosition = _doorRight.Position;
	}


	private void DoBodyEntered(Node3D otherBody)
	{
		if (otherBody.IsInGroup("Characters"))
		{
			ChangeCount(1);
		}
	}

	private void DoBodyExited(Node3D otherBody)
	{
		if (otherBody.IsInGroup("Characters"))
		{
			ChangeCount(-1);
		}
	}

	private void ChangeCount(int d)
	{
		int oldCount = _currentCount;
		_currentCount += d;

		if (oldCount > 0 && _currentCount == 0)
			CloseDoors();

		if (oldCount == 0 && _currentCount > 0)
			OpenDoors();
	}

	private void OpenDoors()
	{
		_doorTween?.Stop();

		_doorTween = CreateTween();
		_doorTween.SetParallel(true);
		_doorTween.SetEase(Tween.EaseType.Out);
		_doorTween.SetTrans(Tween.TransitionType.Circ);
		
		_doorTween.TweenProperty(_doorLeft, "position:x", _leftClosedPosition.X + 1.0f, 0.3f);
		_doorTween.TweenProperty(_doorRight, "position:x", _rightClosedPosition.X - 1.0f, 0.3f);
	}

	private void CloseDoors()
	{
		_doorTween?.Stop();

		_doorTween = CreateTween();
		_doorTween.SetParallel(true);
		_doorTween.SetEase(Tween.EaseType.In);
		_doorTween.SetTrans(Tween.TransitionType.Circ);
		
		_doorTween.TweenProperty(_doorLeft, "position:x", _leftClosedPosition.X, 0.3f);
		_doorTween.TweenProperty(_doorRight, "position:x", _rightClosedPosition.X, 0.3f);
	}
	
}
