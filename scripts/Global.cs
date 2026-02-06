using System;
using Godot;


public partial class Global : Node
{
	public static Global Instance
	{ private set; get; }


	[Export]
	private PackedScene objectPlacer;



	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	public static Node3D GetPlayerNode()
	{
		return Instance.GetTree().GetNodesInGroup("PlayerGroup")[0] as Node3D;
	}

	public static ObjectPlacer TryPlaceObject(FurnitureData data)
	{
		var newPlacer = Instance.objectPlacer.Instantiate<ObjectPlacer>();
		newPlacer.InitWithPrefab(data);
		Instance.GetTree().Root.AddChild(newPlacer);
		return newPlacer;
	}

	public static ObjectPlacer TryMoveObject(Node3D node)
	{
		var newPlacer = Instance.objectPlacer.Instantiate<ObjectPlacer>();
		newPlacer.InitWithExistingObject(node);
		Instance.GetTree().Root.AddChild(newPlacer);
		return newPlacer;
	}
}