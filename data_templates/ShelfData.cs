using Godot;


[GlobalClass]
public partial class ShelfData : FurnitureData
{
	[Export]
	Godot.Collections.Array<ItemData> items;
}
