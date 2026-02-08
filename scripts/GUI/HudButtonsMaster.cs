using Godot;


public partial class HudButtonsMaster : Node
{
	[Export]
	Godot.Collections.Dictionary<Button, PackedScene> _buttonsAndWindows = [];



	public override void _Ready()
	{
		base._Ready();

		foreach (var buttonWindow in _buttonsAndWindows)
		{
			buttonWindow.Key.ButtonUp += () => { OpenWindowByButton(buttonWindow.Key);};
		}
	}

	public void OpenWindowByButton(Button button)
	{
		var newWindow = _buttonsAndWindows[button].Instantiate<Control>();
		HUDLogic.Root.AddChild(newWindow);
	}
}
