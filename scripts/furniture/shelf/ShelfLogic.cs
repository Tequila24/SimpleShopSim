using Godot;
using System;
using System.Linq;


public partial class ShelfLogic : Node3D
{
	[Export]
	private Godot.Collections.Dictionary<Node3D, ItemData> _slots = [];

	public Action OnAnyItemUpdated;



	public override void _Ready()
	{
		base._Ready();
	
		foreach (var slot in _slots)
		{
			if (slot.Value != null)
				UpdateVisualHolder(slot.Key);
		}
	}

	public int GetSlotCount()
	{
		return _slots.Count;
	}

	public void AddItemTo(int index, ItemData newItem)
	{
		_slots[_slots.ElementAt(index).Key] = newItem;
		UpdateVisualHolder(_slots.ElementAt(index).Key);
	}

	public ItemData GetItemAt(int index)
	{
		return _slots.ElementAt(index).Value;
	}

	public void RemoveItemAt(int index)
	{
		AddItemTo(index, null);
	}

	public bool DoesHaveItem(ItemData item)
	{
		return _slots.Values.Contains(item);
	}

	public bool TryTakeItem(ItemData item)
	{
		var match = _slots.FirstOrDefault((v) => v.Value == item);
		if (match.Key != null)
		{
			_slots[match.Key] = null;
			UpdateVisualHolder(match.Key);
			return true;
		} else
		{
			return false;
		}
	}

	private void UpdateVisualHolder(Node3D _slotVisualHolder)
	{
		Utils.ClearChildren(_slotVisualHolder);

		if (_slots[_slotVisualHolder] == null)
			return;

		var newVisual = _slots[_slotVisualHolder].subScene.Instantiate<Node3D>();
		_slotVisualHolder.AddChild(newVisual);
	}
}
