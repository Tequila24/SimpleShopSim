using Godot;
using System;

public partial class DroppedItem : Node3D
{
	[Export]
	private Node3D _visualHolder;

	[Export]
	private ItemData _item;
	public ItemData Item
	{
		get { return _item; }
		set { _item = value; TryUpdateVisual(); }
	}

	private Node3D instantiatedSubScene;



	public override void _Ready()
	{
		base._Ready();

		TryUpdateVisual();
	}

	public void TryUpdateVisual()
	{
		if (_item == null)
			return;

		Utils.ClearChildren(_visualHolder);

		instantiatedSubScene = _item.subScene.Instantiate<Node3D>();
		_visualHolder.AddChild(instantiatedSubScene);
	}

	public void DoItemPickedUp()
	{
		// _visualHolder.RemoveChild(instantiatedSubScene);
		this.QueueFree();
		// return instantiatedSubScene;
	}

	public override void _Process(double delta)
	{
		_visualHolder.Rotate(Vector3.Up, Mathf.RadToDeg(0.02f * (float)delta));
	}
}
