using Godot;


public partial class GlobalPopupMaster : Node
{
	public static GlobalPopupMaster Instance
	{ private set; get; }

	[Export]
	private PackedScene _popupPrefab;
	[Export]
	private VBoxContainer _vBox;



	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}

	public static void ShowPopup(string text)
	{
		var newPopup = Instance._popupPrefab.Instantiate<Control>();
		
		if (newPopup.FindChild("Label") is Label label)
		{
			label.Text = text;
		}

		Instance._vBox.AddChild(newPopup);
		Instance._vBox.MoveChild(newPopup, 0);

		var popupTimer = new Timer();
		newPopup.AddChild(popupTimer);
		popupTimer.Timeout += () => { newPopup.QueueFree(); };
		popupTimer.OneShot = true;
		popupTimer.Start(3);
	}
}
