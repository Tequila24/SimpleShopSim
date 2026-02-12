using Godot;


public interface IAutoExchange
{
	public ItemData GetAutoNextItem();
	public ItemData TryAutoTakeItem();
	public bool TryAutoPutItem(ItemData newItem);
}
