using Godot;


[GlobalClass]
public partial class ItemCountData : Resource
{
	[Export]
	public ItemData item = null;
	[Export]
	public int count = 0;

	

	public ItemCountData()
	{
	}
	
	public ItemCountData(ItemData newItem, int newCount)
	{
		item = newItem;
		count = newCount;
	}
}
