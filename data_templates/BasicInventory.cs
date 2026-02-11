using Godot;
using System.Linq;


[GlobalClass]
public partial class BasicInventory : AInventory
{
	// public bool AddItem(ItemData newItem)
	// {
	// 	var existingItem = _itemsStack.First(itemCount => itemCount.item == newItem);
	// 	if (existingItem != null) 
	// 		existingItem.count++;
	// 	else
	// 		_itemsStack.Add(new ItemCountData(newItem, 1));

	// 	return true;
	// }

	// public ItemData PeekTopItem()
	// {
	// 	if (IsEmpty)
	// 		return null;

	// 	return PeekItemAt(_itemsStack.Count - 1);
	// }

	// public ItemData PeekItemAt(int index)
	// {
	// 	return _itemsStack.ElementAt(index).item;
	// }
}
