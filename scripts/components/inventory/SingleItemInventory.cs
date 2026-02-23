using Godot;
using System.Linq;


public partial class SingleItemInventory : AInventory, IAutoExchange
{
	public ItemData CurrentItemFilter => _data.Content.Count == 0 ? null : _data.Content[0].item;

	public new int Count => CurrentItemFilter == null ? 0 : _data.Content[0].count;

	public new bool IsFull => MaxCapacity > 0 && Count >= MaxCapacity;
	public new bool IsEmpty => CurrentItemFilter == null ? true : _data.Content[0].count == 0;
	public new int AvailableCapacity => MaxCapacity < 0 ? int.MaxValue : MaxCapacity - Count;


	public void InitWithData(InventoryData newData)
	{
		_data = newData;
	}

	public override void _Ready()
	{
		base._Ready();

		InvokeUpdated();
	}

	public override bool TryAddItem(ItemCountData newItemCount)
	{
		if (IsFull)
			return false;

		if (IsEmpty)
		{
			if (AvailableCapacity < newItemCount.count)
				return false;

			_data.Content.Add(newItemCount);
		}
		else
		{
			if (CurrentItemFilter != newItemCount.item)
				return false;

			if (AvailableCapacity < newItemCount.count)
				return false;

			_data.Content[0].count += newItemCount.count;
		}

		InvokeItemUpdated(_data.Content[0]);

		return true;
	}

	public override bool TryRemoveItem(ItemCountData itemCountToTake)
	{
		return false;
	}

	public override int GetItemCount(ItemData item)
	{
		var existingStack = _data.Content.FirstOrDefault<ItemCountData>(nextItem => nextItem.item == item);
		if (existingStack == null)
			return 0;

		return existingStack.count;
	}

	public bool TryPop()
	{
		if (IsEmpty)
			return false;

		_data.Content[0].count -= 1;

		var updatedItem = _data.Content[0].Duplicate() as ItemCountData;

		if (IsEmpty)
			_data.Content.RemoveAt(0);

		InvokeItemUpdated(updatedItem);

		return true;
	}

	public override Godot.Collections.Array<ItemCountData> GetAllItems()
	{
		return _data.Content;
	}

	/* = = = IAutoExchange = = = */
	public bool HasAutoItem(ItemCountData itemCountToFind)
	{
		if (CurrentItemFilter != itemCountToFind.item)
			return false;

		return _data.Content[0].count >= itemCountToFind.count;
	}

	public Godot.Collections.Array<ItemData> GetAutoAvailableItems()
	{
		return [CurrentItemFilter];
	}

	public bool TryAutoTakeItem(ItemCountData unused)
	{
		if (IsEmpty)
			return false;

		return TryPop();
	}

	public bool TryAutoPutItem(ItemCountData newItemCount)
	{
		return TryAddItem(newItemCount);
	}
}
