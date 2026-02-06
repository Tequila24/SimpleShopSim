using System;
using Godot;


public partial class SignalBus : Node
{
	public static SignalBus Instance
	{ private set; get; }

	public static Action<bool> OnShopStateSwitch;

	public static Action<Node3D> OnFurniturePickedUp;
	public static Action<Node3D> OnFurniturePlaced;
}	
