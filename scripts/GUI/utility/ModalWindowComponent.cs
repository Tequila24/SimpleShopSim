using Godot;


[GlobalClass]
public partial class ModalWindowComponent : Node
{
	private Control _parentWindow;


	
	public override void _Ready()
	{
		_parentWindow = this.GetParent<Control>();
		if (_parentWindow == null)
		{
			GD.Print($"Parent window not found: {this.GetParent().Name}");
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton mouseEvent && mouseEvent.IsReleased() && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			bool isInsideControl = _parentWindow.GetGlobalRect().HasPoint(mouseEvent.GlobalPosition);
			// bool isInsideControl = this.HasFocus();

			if (!isInsideControl)
			{
				DoCloseWindow();
			}
		}
	}

	protected void DoCloseWindow()
	{
		_parentWindow.QueueFree();
	}

}