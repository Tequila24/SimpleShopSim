using Godot;
using System;

public partial class NavigationMaster : NavigationRegion3D
{
	public static NavigationMaster Instance
	{ private set; get; }



	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}


	public override void _Ready()
	{
		base._Ready();

		NavigationServer3D.SetDebugEnabled(true);

		BakeFinished += DoBakeFinished;

		BakeNavigationMesh();
	}

	private void DoBakeFinished()
	{
		GD.Print($"Polygon Count: {NavigationMesh.GetPolygonCount()}");
		GD.Print($"Bake Finished!");
	}
}
