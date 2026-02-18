using System.Dynamic;
using Godot;


public partial class SingleItemInventory : Node, IAutoExchange
{
	[Export]
	private InventoryData _data;

	public ItemData CurrentItemFilter => _data.Content.Count == 0 ? null : _data.Content[0].item;

	public int MaxCapacity => _data.MaxCapacity;

	public int Count => CurrentItemFilter == null ? 0 : _data.Content[0].count;
	
	public bool IsFull => MaxCapacity > 0 && Count >= MaxCapacity;
	public bool IsEmpty => CurrentItemFilter == null ? true : _data.Content[0].count == 0;
	public int AvailableCapacity => MaxCapacity < 0 ? int.MaxValue : MaxCapacity - Count;

	[Signal]
	public delegate void OnUpdatedEventHandler();
	public void InvokeUpdated() => EmitSignal(SignalName.OnUpdated);



	public void InitWithData(InventoryData newData)
	{
		_data = newData;
	}

	public override void _Ready()
	{
		_data ??= new();
	}

	public bool TryPush(ItemCountData newItemCount)
	{
		if (IsFull)
			return false;

		if (CurrentItemFilter == null)
		{
			_data.Content[0].item = newItemCount.item;	
		} else
		{
			if (CurrentItemFilter != newItemCount.item)
				return false;
		}

		if (AvailableCapacity < newItemCount.count)
			return false;

		_data.Content[0].count += newItemCount.count;

		GD.Print($"New Item {newItemCount.item.name} Added to {this.Name}");

		return true;
	}

	public bool TryPop()
	{
		if (IsEmpty)
			return false;

		_data.Content[0].count -= 1;
		
		if (IsEmpty)
			_data.Content[0].item = null;

		return true;
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
		return TryPush(newItemCount);
	}
}
