using Godot;
using System;

public partial class NavigationMaster : Node3D
{
	[Export]
	NavigationRegion3D _navRegion;



	public override void _Ready()
	{
		base._Ready();

		_navRegion.BakeFinished += DoBakeFinished;
	
		ReBakeNavmesh();
	}


	public void ReBakeNavmesh()
	{
		_navRegion.BakeNavigationMesh();
	}

	private void DoBakeFinished()
	{
		GD.Print($"Bake Finished!");
	}
}
