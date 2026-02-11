using Godot;


[GlobalClass]
public partial class ItemCountData : Resource
{
	[Export]
	public ItemData item;
	[Export]
	public int count;

	

	public ItemCountData()
	{
		
	}
	
	public ItemCountData(ItemData newItem, int newCount)
	{
		item = newItem;
		count = newCount;
	}
}
