using System;
using Godot;


public partial class ShelfWindowLogic : Control
{
	[Export]
	private PackedScene _itemButtonPrefab;
	[Export]
	private VBoxContainer _buttonsContainer; 

	private ShelfLogic _shelf;

	public Action<int> OnButtonPressed;


	public override void _Ready()
	{
		_shelf = Utils.GetParentOfType<ShelfLogic>(this);
		GD.Print($"Shelf found {_shelf.Name}");
		_shelf.OnAnyItemUpdated += UpdateItemsVisual;

		Init();
		UpdateItemsVisual();
	}

	private void Init()
	{
		if (_shelf == null)
			return;

		for (int idx = 0; idx < _shelf.GetSlotCount(); idx++)
		{
			var newButton = _itemButtonPrefab.Instantiate<Control>();
			_buttonsContainer.AddChild(newButton);

			var button = Utils.GetFirstChildOfType<Button>(newButton);

			int capturedIdx = idx;
			button.ButtonUp += () => { 
				DoItemButtonPressed(capturedIdx); 
			};
		}
	}

	private void UpdateItemsVisual()
	{
		int idx = 0;
		foreach (var child in _buttonsContainer.GetChildren())
		{
			var item = _shelf.GetItemAt(idx);
			if (item == null) {
				idx++;
				continue;
			}

			var itemButton = child as ItemButtonLogic;
			if (itemButton == null)
				continue;

			itemButton.itemNameLabel.Text = item.name;

			idx++;
		}
	}

	private void DoItemButtonPressed(int idx)
	{
		Node playerNode = GetTree().GetNodesInGroup("Player")[0];
		var pickupControl = Utils.GetFirstChildOfType<PickupController>(playerNode);

		var newItem = pickupControl.TryTakeItem();
		if (newItem == null)
			return;

		_shelf.AddItemTo(idx, newItem);
		UpdateItemsVisual();
	}

}
