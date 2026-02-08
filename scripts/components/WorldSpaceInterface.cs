using Godot;


public partial class WorldSpaceInterface : Node
{
	[Export]
	private Interactive _interactive;
	[Export]
	private PackedScene _interfacePrefab;

	private Control _instantiatedScene;
	
	[Export]
	private Node3D _parentObject;



	public override void _Ready()
	{
		_parentObject ??= this.GetParent<Node3D>();

		_interactive.OnEntered += DoOpenUI;
		_interactive.OnExited += DoCloseUI;
	}

	private void DoOpenUI(Node3D unusedNodeEntered)
	{
		_instantiatedScene = _interfacePrefab.Instantiate<Control>();
		_parentObject.AddChild(_instantiatedScene);
		_instantiatedScene.TreeExiting += () => { this._instantiatedScene = null; };
	}

	private void DoCloseUI(Node3D unusedNodeExited)
	{
		if (_instantiatedScene == null)
			return;

		_instantiatedScene.QueueFree();
	}

	public override void _Process(double delta)
	{
		if (_instantiatedScene == null)
			return;

		_instantiatedScene.GlobalPosition = CameraMaster.GetCurrentCamera().UnprojectPosition(_parentObject.GlobalPosition);
	}
}
