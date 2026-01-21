using Godot;
using System;


public partial class ShelfLogic : Node3D
{
	[Export]
	private Node SlotsHolder;
	private Godot.Collections.Array<Node> _itemsVisualHolders;

	[Export]
	private ItemData[] _itemsOnShelf;

	public Action OnAnyItemUpdated;
	

	public override void _Ready()
	{
		InitShelfData();
	}

	private void InitShelfData()
	{
		int slotsCount = SlotsHolder.GetChildCount();
		_itemsVisualHolders = SlotsHolder.GetChildren();
		_itemsOnShelf = new ItemData[slotsCount];
	}

	public int GetSlotCount()
	{
		return _itemsVisualHolders.Count;
	}

	public void AddItemTo(int index, ItemData newItem)
	{
		_itemsOnShelf[index] = newItem;
		UpdateVisualHolder(index);
	}

	public ItemData GetItemAt(int index)
	{
		return _itemsOnShelf[index];
	}

	public void RemoveItemAt(int index)
	{
		_itemsOnShelf[index] = null;
		UpdateVisualHolder(index);
	}

	private void UpdateVisualHolder(int index)
	{
		Utils.ClearChildren(_itemsVisualHolders[index]);

		if (_itemsOnShelf[index] == null)
			return;

		var newVisual = _itemsOnShelf[index].subScene.Instantiate<Node3D>();
		_itemsVisualHolders[index].AddChild(newVisual);
	}
}
