using System;
using System.Collections;
using Godot;


[GlobalClass]
public partial class StackInventory : Resource
{
	[Export]
	private int _maxCapacity = -1;
	public int MaxCapacity => _maxCapacity;
	
	public int Count => stack.Count;

	public bool IsFull => (_maxCapacity > 0 && stack.Count >= _maxCapacity);
	public bool IsEmpty => (Count == 0);

	[Export]
	private Godot.Collections.Array<ItemData> stack = [];

	[Signal]
	public delegate void OnUpdatedEventHandler();
	public void EmitOnUpdated()
	{ EmitSignal(SignalName.OnUpdated); }



	public bool PushItem(ItemData newItem)
	{
		if (_maxCapacity > 0 && stack.Count >= _maxCapacity) {
			GD.Print($"Tshis stack is at max capacity");
			return false;
		}

		stack.Add(newItem);
		EmitOnUpdated();
		return true;
	}

	public ItemData TryPopItem()
	{
		if (Count <= 0)
			return null;
		
		var retValue = stack[Count - 1];
		stack.RemoveAt(Count - 1);
		EmitOnUpdated();

		return retValue;
	}

	public ItemData PeekTopItem()
	{
		if (IsEmpty)
			return null;

		return PeekItemAt(stack.Count - 1);
	}

	public ItemData PeekItemAt(int index)
	{
		if (IsEmpty)
			return null;

		if (Mathf.Abs(index) > stack.Count)
			return null;
			
		// GD.Print($"Index {index}");
		return stack[index >= 0 ? index : Count - index];
	}
}
