using Godot;


[GlobalClass]
public partial class ItemCountData : Resource
{
	[Export]
	public ItemData item;
	[Export]
	public int count;
}
