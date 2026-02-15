using Godot;
using System;


public partial class NPCSpawner : Node
{
	static public NPCSpawner Instance
	{ private set; get; }

	[Export]
	public Node3D _NPCSpawnPoint;
	[Export]
	public Node3D _NPCDespawnPoint;
	[Export]
	private PackedScene _npcPrefab;
	[Export]
	private Godot.Collections.Array<ItemData> _randomWantedItemList = [];
	[Export]
	private Node3D _currentNPC;


	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}


	public override void _Ready()
	{
		base._Ready();

		CallDeferred("SpawnNewNPC");
	}

	private void SpawnNewNPC()
	{
		_currentNPC = _npcPrefab.Instantiate<Node3D>();
		_currentNPC.TreeExited += OnCurrentNPCRemoved;

		if (Utils.GetFirstChildOfType<BuyerLogic>(_currentNPC) is BuyerLogic bLogic)
		{
			bLogic._wantedItems.Add(_randomWantedItemList.PickRandom());
		}

		_currentNPC.Position = _NPCSpawnPoint.GlobalPosition;
		GetTree().Root.AddChild(_currentNPC);
	}

	private void OnCurrentNPCRemoved()
	{
		_currentNPC = null;
		SpawnNewNPC();
	}
}
