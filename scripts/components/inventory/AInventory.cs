using System;
using System.Linq;
using Godot;


public abstract partial class AInventory : Node
{
	[Export]
	protected InventoryData _data;

	public int MaxCapacity => _data.MaxCapacity;

	public int Count => _data.Content.Count;
	
	public bool IsFull => MaxCapacity > 0 && Count >= MaxCapacity;
	public bool IsEmpty => _data.Content.Count == 0;
	public int AvailableCapacity => MaxCapacity < 0 ? int.MaxValue : MaxCapacity - Count;

	[Signal]
	public delegate void OnUpdatedEventHandler();
	public void InvokeUpdated() => EmitSignal(SignalName.OnUpdated);
	[Signal]
	public delegate void OnItemUpdatedEventHandler(ItemCountData updatedItemCount);
	public void InvokeItemUpdated(ItemCountData updatedItemCount) => EmitSignal(SignalName.OnItemUpdated, updatedItemCount);
	

	public override void _Ready()
	{
		base._Ready();

		_data ??= new();
	}

	public abstract bool TryAddItem(ItemCountData newItemCount);
	public abstract bool TryRemoveItem(ItemCountData itemCountToTake);
	public abstract int GetItemCount(ItemData item);
	public abstract Godot.Collections.Array<ItemCountData> GetAllItems();
}