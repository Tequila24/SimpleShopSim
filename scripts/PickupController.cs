using System.Collections;
using System.Linq;
using Godot;


public partial class PickupController : Node
{
	[Export]
	private Area3D _pickupArea;
	[Export]
	private Node _itemVisualHolder;

	[Export]
	private ItemData _itemHolding;


	public override void _Ready()
	{
		base._Ready();

		_pickupArea.AreaEntered += DoAreaEntered;
		
		InputMaster.Instance.OnDropKeyPressed += DoDropItem;
	}

	private void DoAreaEntered(Area3D otherArea)
	{
		var otherAreaParent = otherArea.GetParent() as DroppedItem;
		if (otherAreaParent== null)
			return;

		// GD.Print($"Area Entered {otherAreaParent.Name}");

		if (otherAreaParent.Item.IsItemsBox())
			DoPickupItemsBox(otherAreaParent.Item as ItemsBoxData);

		otherAreaParent.DoItemPickedUp();
	}

	private void DoPickupItemsBox(ItemsBoxData newBox)
	{
		_itemHolding = newBox;
		_itemVisualHolder.AddChild(newBox.subScene.Instantiate());

		GD.Print($"Picked up box of");
		foreach (var itemCount in newBox.content)
		{
			GD.Print($"Item: {itemCount.item.name}, amount: {itemCount.count}");
		}
	}

	private void DoDropItem()
	{
		_itemHolding = null;
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		Utils.ClearChildren(_itemVisualHolder);

		if (_itemHolding == null)
			return;
			
		_itemVisualHolder.AddChild(_itemHolding.subScene.Instantiate<Node3D>());
	}

	public ItemData TryTakeItem()
	{
		ItemData retValue = null;

		switch (@_itemHolding)
		{
			case ItemsBoxData box:
				{
					if (box.content.Count <= 0)
						return null;

					var topItemCount = box.content[box.content.Count - 1];
					if (topItemCount.count > 1) {
						topItemCount.count--;
						retValue = topItemCount.item;
					} else
					{
						retValue = topItemCount.item;
						box.content.RemoveAt(box.content.Count - 1);
					}
				}
			break;

			default:
			break;
		}

		return retValue;
	}
}
