using Godot;


[GlobalClass]
public partial class AccountData : Resource
{
	[Export]
	public ItemCountData money;

	[Export]
	public Godot.Collections.Dictionary<FurnitureData, bool> furnitureUnlocks;
}