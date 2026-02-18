using System;
using Godot;


public partial class StackInventoryOld : InventoryData
{
	// public bool TryPush(ItemCountData newItemCount)
	// {
	// 	if (IsFull)
	// 		return false;

	// 	if (AvailableCapacity < newItemCount.count)
	// 		return false;

	// 	for (int i = 0; i < newItemCount.count; i++)
	// 	{
	// 		TryAddItem(new (newItemCount.item, 1));
	// 	}

	// 	return true;
	// }

	// public ItemCountData TryPop()
	// {
	// 	var retValue = TryGetItem(-1);
		
	// 	if (retValue != null)
	// 		DeleteItem(-1);

	// 	return retValue;
	// }

	// public ItemCountData TryPeekTop()
	// {
	// 	return TryPeekAt(Count - 1);
	// }

	// public ItemCountData TryPeekAt(int index)
	// {
	// 	return TryGetItem(index);
	// }

	// public ItemCountData PeekAutoNextItem()
	// {
	// 	return TryPeekTop();
	// }

	// public ItemCountData TryAutoTakeItem()
	// {
	// 	return TryPop();
	// }

	// public bool TryAutoPutItem(ItemCountData newItem)
	// {
	// 	return TryPush(newItem);
	// }
}
