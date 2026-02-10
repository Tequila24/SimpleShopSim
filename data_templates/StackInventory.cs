using Godot;


[GlobalClass]
public partial class StackInventory : BaseInventory
{	
	public bool PushItem(ItemData newItem)
	{
		if (_maxCapacity > 0 && _itemsStack.Count >= _maxCapacity) {
			return false;
		}

		_itemsStack.Add(newItem);
		EmitOnUpdated();
		return true;
	}

	public ItemData TryPopItem()
	{
		if (Count <= 0)
			return null;
		
		var retValue = _itemsStack[Count - 1];
		_itemsStack.RemoveAt(Count - 1);
		EmitOnUpdated();

		return retValue;
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
			
		return _itemsStack[index >= 0 ? index : Count - index];
	}
}
