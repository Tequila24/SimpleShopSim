using System;
using System.Linq;
using Godot;


public partial class BasicInventory : AInventory, IAutoExchange
{
	public override void _Ready()
	{
		base._Ready();

		InvokeUpdated();
	}

	public override bool TryAddItem(ItemCountData newItemCount)
	{
		var existingStack = _data.Content.FirstOrDefault(itemCount => itemCount.item == newItemCount.item);

		if (existingStack == null)
		{
			var newEntry = newItemCount.Duplicate() as ItemCountData;
			_data.Content.Add(newEntry);
			InvokeItemUpdated(newEntry);
		}
		else
		{
			existingStack.count += newItemCount.count;
			InvokeItemUpdated(existingStack);
		}

		return true;
	}

	public override bool TryRemoveItem(ItemCountData itemCountToTake)
	{
		var existingStack = _data.Content.First(itemCount => itemCount.item == itemCountToTake.item);

		if (existingStack == null)
		{
			return false;
		}

		if (existingStack.count < itemCountToTake.count)
		{
			return false;
		}

		existingStack.count -= itemCountToTake.count;

		if (existingStack.count == 0)
			_data.Content.Remove(existingStack);

		InvokeItemUpdated(existingStack);

		return true;
	}

	public override int GetItemCount(ItemData item)
	{
		var existingStack = _data.Content.FirstOrDefault<ItemCountData>(nextItem => nextItem.item == item);
		if (existingStack == null)
			return 0;

		return existingStack.count;
	}

	public override Godot.Collections.Array<ItemCountData> GetAllItems()
	{
		return _data.Content;
	}

	/* = = = IAutoExchange = = = */
	public bool HasAutoItem(ItemCountData itemCountToFind)
	{
		if (IsEmpty)
			return false;

		GD.Print($"Content:");
		foreach (var stack in _data.Content)
		{			
			GD.Print($"\tItem {stack.item} Count {stack.count}");
		}

		var existingStack = _data.Content.FirstOrDefault(itemCount => itemCount.item == itemCountToFind.item);

		if (existingStack == null)
			return false;

		return existingStack.count >= itemCountToFind.count;
	}

	public Godot.Collections.Array<ItemData> GetAutoAvailableItems()
	{
		var sharpArray = _data.Content.Select(itemStack => itemStack.item).ToList();

		return [.. sharpArray];
	}

	public bool TryAutoTakeItem(ItemCountData itemCountToTake)
	{
		return TryRemoveItem(itemCountToTake);
	}

	public bool TryAutoPutItem(ItemCountData newItemCount)
	{
		return TryAddItem(newItemCount);
	}

}