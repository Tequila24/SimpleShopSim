using Godot;


public partial class PopupController : Node
{
	[Export]
	private PackedScene _windowScene;
	[Export]
	private Area3D _interactionArea;

	
	private Control _instantiatedScene;
	private Node3D _parentObject;



	public override void _Ready()
	{
		base._Ready();
	
		_parentObject ??= this.GetParent<Node3D>();
		_interactionArea.BodyEntered += DoBodyEntered;
		_interactionArea.BodyExited += DoBodyExited;
	}

	private void DoBodyEntered(Node3D body)
	{
		if (body == GetTree().GetNodesInGroup("Player")[0] as Node3D) {
			_instantiatedScene = _windowScene.Instantiate<Control>();
			_parentObject.AddChild(_instantiatedScene);
		}
	}

	private void DoBodyExited(Node3D body)
	{
		if (_instantiatedScene != null) {
			_instantiatedScene.QueueFree();
			_instantiatedScene = null;
		}
	}

	public override void _Process(double delta)
	{
		if (_instantiatedScene != null)
		 	_instantiatedScene.GlobalPosition = CameraMaster.GetCurrentCamera().UnprojectPosition(_parentObject.Position);

	}
}
