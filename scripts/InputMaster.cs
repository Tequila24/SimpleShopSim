using System;
using Godot;


public partial class InputMaster : Node
{
	public static InputMaster Instance
	{
		private set; get;
	}
	
	public Action<Vector3> OnPlayerDirectionUpdated;
	public Action<float> OnZoomCamera;
	public Action OnDropKeyPressed;

	[Signal]
	public delegate void OnPlayerRotationUpdatedEventHandler(Vector2 deltaRotationVector);



	public override void _EnterTree()
	{
		base._EnterTree();

		// Input.MouseMode = Input.MouseModeEnum.Captured;
		// GD.Print($" Input Mode {Input.MouseMode}");

		Instance ??= this;
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
			Vector3 inputDir = new Vector3(Input.GetAxis("right", "left"), 0.0f, Input.GetAxis("down", "up"));
			OnPlayerDirectionUpdated(inputDir.Normalized());
		}

		if (IsReleaseEvent(eventKey, Key.Escape))
		{
			// Input.MouseMode = Input.MouseModeEnum.Visible;
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
		// if (Input.MouseMode == Input.MouseModeEnum.Visible)
		// 	Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void ProcessMouseMotionEvent(InputEventMouseMotion eventMouseMotion)
	{
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
			return;
	}
}
