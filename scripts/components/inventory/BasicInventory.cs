using System.Linq;
using Godot;


public partial class BasicInventory : Node, IAutoExchange
{
	[Export]
	private InventoryData _data;



	public override void _Ready()
	{
		_data ??= new();
	}

	public bool AddItem(ItemCountData newItemCount)
	{
		var existingStack = _data.Content.First(itemCount => itemCount.item == newItemCount.item);

		if (existingStack == null)
		{
			_data.Content.Add(newItemCount.Duplicate() as ItemCountData);
		}
		else
		{
			existingStack.count += newItemCount.count;
		}

		return true;
	}

	public bool RemoveItem(ItemCountData itemCountToTake)
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

		return true;
	}

	/* = = = IAutoExchange = = = */
	public bool HasAutoItem(ItemCountData itemCountToFind)
	{
		var existingStack = _data.Content.First(itemCount => itemCount.item == itemCountToFind.item);

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
		return RemoveItem(itemCountToTake);
	}

	public bool TryAutoPutItem(ItemCountData newItemCount)
	{
		return AddItem(newItemCount);
	}
}