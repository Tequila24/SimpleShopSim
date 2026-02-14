using Godot;


public interface IAutoExchange
{
	public ItemData PeekAutoNextItem();
	public ItemData TryAutoTakeItem();
	public bool TryAutoPutItem(ItemData newItem);
}
