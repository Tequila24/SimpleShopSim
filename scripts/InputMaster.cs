using System;
using Godot;


public partial class InputMaster : Node
{
	public static InputMaster Instance
	{ private set; get; }

	public enum InputState
	{
		PLAYER_CONTROLS,
		OBJECT_PLACING
	}
	private InputState _currentInputState;
	public InputState CurrentInputState
	{
		get { return _currentInputState; }
		set
		{
			_currentInputState = value;
			DoInputStateChanged();
		}
	}
	
	public Action<Vector3> OnPlayerDirectionUpdated;
	public Action<float> OnZoomCamera;
	public Action<float> OnRotateCamera;
	public Action OnDropKeyPressed;

	public Action<Vector2> OnDragUpdated;
	public Action OnTap;
	public Action OnDoubleTap;

	bool _middleMouseButtonHeld = false;
	Timer _doubleTapTimer;
	bool _doubleTapFlag = false;

	[Signal]
	public delegate void OnPlayerRotationUpdatedEventHandler(Vector2 deltaRotationVector);



	public override void _EnterTree()
	{
		base._EnterTree();

		// Input.MouseMode = Input.MouseModeEnum.Captured;
		// GD.Print($" Input Mode {Input.MouseMode}");

		Instance ??= this;
		_currentInputState = InputState.PLAYER_CONTROLS;
	}


	public override void _Input(InputEvent inputEvent)
	{
		switch (@inputEvent)
		{
			case InputEventKey eventKey:
				ProcessKeyEvent(eventKey);
				break;
			case InputEventMouseButton eventMouse:
				ProcessMouseButtonEvent(eventMouse);
				break;
			case InputEventMouseMotion eventMouseMotion:
				ProcessMouseMotionEvent(eventMouseMotion);
				break;
		}
	}

	private void ProcessKeyEvent(InputEventKey eventKey)
	{
		if (!eventKey.IsEcho()) {

			if (_currentInputState == InputState.PLAYER_CONTROLS) {
				Vector3 inputDir = new Vector3(Input.GetAxis("right", "left"), 0.0f, Input.GetAxis("down", "up"));
				OnPlayerDirectionUpdated(inputDir.Normalized());
			}
		}

		if (IsReleaseEvent(eventKey, Key.Escape))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		if (IsReleaseEvent(eventKey, Key.Ctrl))
			OnDropKeyPressed();

		// if (IsTapEvent(eventKey, Key.E))
		// {
		// 	EmitSignal(SignalName.OnPlayerInteract);
		// }
	}

	private bool IsPressEvent(InputEventKey keyEvent, Key key)
	{
		return keyEvent.Keycode == key && keyEvent.IsPressed() && keyEvent.Echo == false;
	}

	private bool IsReleaseEvent(InputEventKey keyEvent, Key key)
	{
		return keyEvent.Keycode == key && keyEvent.IsReleased() && keyEvent.Echo == false;
	}

	private void ProcessMouseButtonEvent(InputEventMouseButton eventMouseButton)
	{
		if (eventMouseButton.ButtonIndex == MouseButton.WheelUp)
		{
			OnZoomCamera(-1.0f);
		}
		if (eventMouseButton.ButtonIndex == MouseButton.WheelDown)
		{
			OnZoomCamera(+1.0f);
		}
		if (eventMouseButton.ButtonIndex == MouseButton.Middle)
		{
			_middleMouseButtonHeld = eventMouseButton.IsPressed();
		}

		if (_currentInputState == InputState.OBJECT_PLACING)
		{
			if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed && eventMouseButton.IsEcho() == false)
			{
				if (_doubleTapTimer == null)
				{
					_doubleTapTimer = new Timer();
					this.AddChild(_doubleTapTimer);
					_doubleTapTimer.Timeout += DoDoubleTapTimeout;
					_doubleTapTimer.OneShot = true;
					_doubleTapTimer.Start(0.22f);
				} else
				{
					_doubleTapFlag = true;
				}
				
			}
		}

		// if (Input.MouseMode == Input.MouseModeEnum.Visible)
		// 	Input.MouseMode = Input.MouseModeEnum.Captured;
	}



	private void ProcessMouseMotionEvent(InputEventMouseMotion eventMouseMotion)
	{
		// if (Input.MouseMode != Input.MouseModeEnum.Captured)
			// return;

		if (_currentInputState == InputState.OBJECT_PLACING)
			OnDragUpdated(eventMouseMotion.ScreenRelative);

		if(_middleMouseButtonHeld)
			OnRotateCamera(eventMouseMotion.ScreenRelative.X);
	}

	private void DoInputStateChanged()
	{
		switch (_currentInputState)
		{
			case InputState.PLAYER_CONTROLS:
				{
					Input.MouseMode = Input.MouseModeEnum.Visible;
					break;
				}
			
			case InputState.OBJECT_PLACING:
				{
					Input.MouseMode = Input.MouseModeEnum.Captured;
					break;
				}

			
			default:
			break;
		}
	}

	private void DoDoubleTapTimeout()
	{
		if (_doubleTapFlag)
			OnDoubleTap();
		else
			OnTap();

		_doubleTapFlag = false;

		_doubleTapTimer.QueueFree();
		_doubleTapTimer = null;
	}
}
