using System;
using Godot;


public partial class StackInventory : Resource
{
	[Export]
	private Godot.Collections.Array<ItemData> stack = [];

	public Action OnUpdated;

	public int Count()
	{
		return stack.Count;
	}

	public void PushItem(ItemData newItem)
	{
		stack.Add(newItem);

		OnUpdated?.Invoke();
	}

	public ItemData PopItem()
	{
		var retValue = stack[stack.Count - 1];
		stack.RemoveAt(stack.Count - 1);

		OnUpdated?.Invoke();

		return retValue;
	}

	public ItemData PeekTopItem()
	{
		return PeekItemAt(stack.Count - 1);
	}

	public ItemData PeekItemAt(int index)
	{
		if (index < 0 || index > stack.Count)
			return null;
			
		return stack[index];
	}
}
