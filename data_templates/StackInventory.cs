using Godot;


[GlobalClass]
public partial class StackInventory : AInventory, IAutoExchange
{	
	public bool TryPushItem(ItemData newItem)
	{
		if (IsFull) {
			return false;
		}

		_itemsStack.Add(new ItemCountData(newItem, 1));
		EmitOnUpdated();
		return true;
	}

	public ItemData TryPopItem()
	{
		if (IsEmpty)
			return null;
		
		var retValue = _itemsStack[Count - 1];
		_itemsStack.RemoveAt(Count - 1);
		EmitOnUpdated();

		return retValue.item;
	}

	public ItemData PeekTopItem()
	{
		if (IsEmpty)
			return null;

		return PeekItemAt(_itemsStack.Count - 1);
	}

	public ItemData PeekItemAt(int index)
	{
		if (IsEmpty)
			return null;

		if (Mathf.Abs(index) > _itemsStack.Count)
			return null;
			
		return _itemsStack[index >= 0 ? index : Count - index].item;
	}

	public ItemData PeekAutoNextItem()
	{
		return PeekTopItem();
	}

	public ItemData TryAutoTakeItem()
	{
		return TryPopItem();
	}

	public bool TryAutoPutItem(ItemData newItem)
	{
		return TryPushItem(newItem);
	}
}
