using Godot;
using System;


[GlobalClass]
public partial class InventoryData : Resource
{
	[Export]
	public Godot.Collections.Array<ItemCountData> Content = [];

	[Export]
	protected int _maxCapacity = -1;
	public int MaxCapacity => _maxCapacity;



	// protected bool TryAddItem(ItemCountData newItemCount)
	// {
	// 	if (IsFull)
	// 		return false;

	// 	_content.Add(newItemCount);
	// 	InvokeUpdated();
	// 	return true;
	// }

	// protected ItemCountData TryGetItem(int index)
	// {
	// 	if (IsEmpty)
	// 		return null;

	// 	if (Mathf.Abs(index) >= Count)
	// 		return null;

	// 	return _content[index >= 0 ? index : Count - index];
	// }

	// protected bool DeleteItem(int index)
	// {
	// 	if (Mathf.Abs(index) >= Count)
	// 		return false;

	// 	_content.RemoveAt(index >= 0 ? index : Count - index);
	// 	InvokeUpdated();
	// 	return true;
	// }
}
