using System;
using Godot;


public partial class SignalBus : Node
{
	public static SignalBus Instance
	{ private set; get; }

	public static Action<int, int> OnAccMoneyChanged;

	public static Action<bool> OnShopStateSwitch;

	public static Action<Node3D> OnFurniturePickedUp;
	public static Action<Node3D> OnFurniturePlaced;



	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}
}	
