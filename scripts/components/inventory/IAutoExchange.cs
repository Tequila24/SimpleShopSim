using Godot;


public interface IAutoExchange
{
	public Godot.Collections.Array<ItemData> GetAutoAvailableItems();

	public bool HasAutoItem(ItemCountData itemCount);

	public bool TryAutoTakeItem(ItemCountData itemCount);
	public bool TryAutoPutItem(ItemCountData newItemCount);
}
