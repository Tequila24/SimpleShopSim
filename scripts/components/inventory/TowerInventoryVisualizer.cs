using Godot;
using System;

public partial class TowerInventoryVisualizer : Node3D
{
	[Export]
	private Node3D _visualHolder = null;
	[Export]
	private AInventory _inventory = null;

	private Godot.Collections.Dictionary<ItemData, Node3D> _itemsTowers = [];



	public override void _Ready()
	{
		_visualHolder ??= this;
		_inventory ??= Utils.GetFirstChildOfType<AInventory>(this.GetParent());

		_inventory.OnItemUpdated += DoItemUpdated;

		InitInventoryVisual();
	}

	private void InitInventoryVisual()
	{
		var allItems = _inventory.GetAllItems();
		foreach (var item in allItems)
		{
			DoItemUpdated(item);
		}
	}

	private void DoItemUpdated(ItemCountData updatedItemCount)
	{
		Node3D itemTowerHolder;

		// string output = updatedItemCount == null ? "ITEM IS NULL" : updatedItemCount.item.name;
		GD.Print($"ITEM TEST {updatedItemCount.item.name}");

		if (_itemsTowers.ContainsKey(updatedItemCount.item))
		{
			itemTowerHolder = _itemsTowers[updatedItemCount.item];
		}
		else
		{
			itemTowerHolder = new Node3D();

			itemTowerHolder.Name = $"{updatedItemCount.item.name}";
			GD.Print($"{itemTowerHolder.Name}");
			GD.Print($"{_visualHolder.Name}");
			_visualHolder.AddChild(itemTowerHolder);
			_itemsTowers.Add(updatedItemCount.item, itemTowerHolder);
		}

		int countDifference = itemTowerHolder.GetChildCount() - updatedItemCount.count;
		GD.Print($"Difference {countDifference}");

		if (countDifference == 0)
			return;
		else if (countDifference > 0)
		{
			while (countDifference > 0)
			{
				itemTowerHolder.RemoveChild(itemTowerHolder.GetChild(-1));
				countDifference--;
			}
			if (itemTowerHolder.GetChildCount() == 0)
			{
				_itemsTowers.Remove(updatedItemCount.item);
			}
		}
		else if (countDifference < 0)
		{
			while (countDifference < 0)
			{
				var newItemScene = updatedItemCount.item.subScene.Instantiate<Node3D>();

				Vector3 nextPos = itemTowerHolder.GetChildCount() > 0 ? itemTowerHolder.GetChild<Node3D>(-1).Position : new Vector3(0, -0.5f, 0);

				nextPos += new Vector3(0, 0.5f, 0);
				newItemScene.Position = nextPos;

				itemTowerHolder.AddChild(newItemScene);

				countDifference++;
			}
		}
	}
}
