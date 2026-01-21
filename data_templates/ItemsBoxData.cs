using Godot;


[GlobalClass]
public partial class ItemsBoxData : ItemData
{
	[Export]
	public Godot.Collections.Array<ItemCountData> content = new();
}
