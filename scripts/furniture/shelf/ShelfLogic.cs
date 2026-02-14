using Godot;
using System;
using System.Linq;


public partial class ShelfLogic : Node3D, IAutoExchange
{
	[Export]
	private ShelfData _data;

	public ItemData ShelfItem => _data.contents.item;
	public int Capacity => _data.contents.count;

	[Export]
	private Node3D _visualsRoot;
	private Godot.Collections.Array<Node3D> _visualHolders = new();



	public override void _Ready()
	{
		base._Ready();

		foreach (var holder in _visualsRoot.GetChildren())
		{
			_visualHolders.Add(holder as Node3D);
		}

		UpdateVisual();
	}

	public bool TryAddItem(ItemData newItem)
	{
		if (ShelfItem != newItem)
			return false;

		if (_data.contents.count >= _visualHolders.Count)
			return false;

		_data.contents.count++;
		UpdateVisual();
		return true;
	}

	public bool TryTakeItem()
	{
		if (_data.contents.count <= 0)
			return false;

		_data.contents.count--;
		UpdateVisual();
		return true;
	}

	private void UpdateVisual()
	{
		int itemsVisualCount = 0;
		foreach (var vHolder in _visualHolders)
		{
			if (vHolder.GetChildCount() > 0)
				itemsVisualCount++;
		}


		if (_data.contents.count == itemsVisualCount)
		{
			return;
		}
		else if (_data.contents.count < itemsVisualCount)
		{
			int difference = itemsVisualCount - _data.contents.count;
			// GD.Print($"data > visual, difference: {difference}");

			while (difference > 0)
			{
				Utils.ClearChildren(_visualHolders[itemsVisualCount - difference]);
				difference--;
			}
		}
		else if (_data.contents.count > itemsVisualCount)
		{
			int difference = _data.contents.count - itemsVisualCount;

			// GD.Print($"visual > data, difference: {difference} {itemsVisualCount}");

			int i = 0;
			while (i < difference)
			{
				Node3D newItemVisual = ShelfItem.subScene.Instantiate<Node3D>();
				_visualHolders[itemsVisualCount + i].AddChild(newItemVisual);
				i++;
			}
		}
	}

	public ItemData PeekAutoNextItem()
	{
		if (_data.contents.count <= 0)
			return null;

		return _data.contents.item;
	}

	public ItemData TryAutoTakeItem()
	{
		// no auto taking
		return null;
	}

	public bool TryAutoPutItem(ItemData newItem)
	{
		if (newItem != ShelfItem)
		{
			return false;
		}

		return TryAddItem(newItem);
	}
}
