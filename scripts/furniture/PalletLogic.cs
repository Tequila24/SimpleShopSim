using Godot;
using System;
using System.Threading;

public partial class PalletLogic : Node3D
{
	[Export]
	public PalletData PalletData
	{ private set; get; }

	[Export]
	private Node3D _itemsVisualHolder;
	private Vector3 _itemVisualPositionStep = Vector3.One * 0.5f;
	private Vector3 _itemVisualPositionMax = Vector3.One;


	public void InitWithData(PalletData newData)
	{
		PalletData = newData;
		_itemsVisualHolder ??= this.FindChild("ItemsVisualHolder") as Node3D;
	}

	public override void _Ready()
	{
		base._Ready();
		UpdateVisual();
	}

	public ItemData TryTakeItem()
	{
		if (PalletData.Item.count <= 0)
			return null;

		
		PalletData.Item.count--;
		UpdateVisual();
		return PalletData.Item.item;
	}

	public bool TryAddItem(ItemData item)
	{
		if (PalletData.Item.item == item)
		{
			PalletData.Item.count++;	
			UpdateVisual();
			return true;
		}
		else if (PalletData.Item.count == 0)
		{
			PalletData.Item.item = item;
			PalletData.Item.count++;
			UpdateVisual();
			return true;
		}
	
		return false;
	}

	private void UpdateVisual()
	{
		int itemsVisualCount = _itemsVisualHolder.GetChildCount();

		if (PalletData.Item.count == itemsVisualCount)
		{
			return;
		}
		else if (PalletData.Item.count < itemsVisualCount)
		{
			_itemsVisualHolder.GetChild(-1).QueueFree();
		}
		else if (PalletData.Item.count > itemsVisualCount)
		{
			int difference = PalletData.Item.count - itemsVisualCount;

			// if no item, next will be at 0
			Vector3 nextItemVisualPos = new Vector3(-_itemVisualPositionStep.X, 0, 0);
			if (itemsVisualCount > 0) {
				nextItemVisualPos = _itemsVisualHolder.GetChild<Node3D>(-1).Position;
				
			}

			while (difference > 0)
			{
				//next position
				nextItemVisualPos.X += _itemVisualPositionStep.X;
				if (nextItemVisualPos.X > _itemVisualPositionMax.X)
				{
					nextItemVisualPos.X = 0;
					nextItemVisualPos.Z += _itemVisualPositionStep.Z;
					if (nextItemVisualPos.Z > _itemVisualPositionMax.Z) {
						nextItemVisualPos.Z = 0;
						nextItemVisualPos.Y += _itemVisualPositionStep.Y;
					}
				}

				
				Node3D newItemVisual = PalletData.Item.item.subScene.Instantiate<Node3D>();
				_itemsVisualHolder.AddChild(newItemVisual);
				newItemVisual.Position = nextItemVisualPos;

				difference--;
			}
		}
	}
}
